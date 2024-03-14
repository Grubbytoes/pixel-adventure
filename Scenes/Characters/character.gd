extends CharacterBody2D
class_name Character

const TERMINAL_VELOCITY = 300

@export var speed = 12
@export var buddy: Character
@export var currently_controlled = true

var face_right = true
var grav_excempt = false
var grav_v = 0.0
var anim: Anim

@onready var sprite = $sprite
@onready var cam = $cam
@onready var woosh_time = Timer.new()
@onready var pass_control_buffer = Timer.new()


# Animation enum
enum Anim {IDLE, RUN, AIR_UP, AIR_DOWN}

func move_by_input(v: Vector2) -> Vector2:
	var vn = v
	vn.x = 0
	
	if Input.is_action_pressed("ui_left"):
		vn.x -= 1
		anim = Anim.RUN
	if Input.is_action_pressed("ui_right"):
		vn.x += 1
		anim = Anim.RUN
	
	if Input.is_action_just_pressed("ui_right"):
		face_right = true
		$sprite.flip_h = !face_right
	elif Input.is_action_just_pressed("ui_left"):
		face_right = false
		$sprite.flip_h = !face_right
	
	if Input.is_action_just_pressed("ui_up") and is_on_floor():
		vn = jump(vn)
	
	vn.x *= speed * 15
	return vn


func jump(v: Vector2) -> Vector2:
	# WOOSH time
	grav_excempt = true
	woosh_time.wait_time = 0.1
	woosh_time.start()
	
	$anim.play("jump")
	
	var vn = Vector2.ZERO
	vn.y -= 250
	return vn + v
	

func woosh_time_over():
	grav_excempt = false
	

func apply_gravity(v: Vector2) -> Vector2:
	if grav_excempt: return v
	var vn = v
	
	if is_on_floor():
		vn.y = 0.0
	elif vn.y >= TERMINAL_VELOCITY:
		vn.y = TERMINAL_VELOCITY
	else:
		vn.y += 10

	# Animation
	if !is_on_floor() and velocity.y > 0:
		anim = Anim.AIR_DOWN
	elif velocity.y < 0:
		anim = Anim.AIR_UP
	elif velocity == Vector2.ZERO:
		anim = Anim.IDLE
	
	return vn
		

func update_animation():
	match anim:
		Anim.IDLE:
			sprite.play("idle")
		Anim.RUN:
			sprite.play("run")
		Anim.AIR_DOWN:
			sprite.play("air_down")
		Anim.AIR_UP:
			sprite.play("air_up")


func pass_control(to: Character):
	# Just a safeguard:
	if !currently_controlled: return
	print(self.name + " passing control")
	currently_controlled = false

	# Stop any horizontal movement
	velocity.x = 0
	
	# Start the timer
	pass_control_buffer.wait_time = 0.1
	pass_control_buffer.timeout.connect(to.recieve_control.bind(self))
	pass_control_buffer.start()

	# Swap the camera
	to.cam.enabled = true
	cam.enabled = false


func recieve_control(from: Character):
	print(self.name + " recieveing control")
	currently_controlled = true
	from.pass_control_buffer.timeout.disconnect(recieve_control)


func _process(delta):
	if currently_controlled:
		velocity = move_by_input(velocity)
		if Input.is_action_just_pressed("ui_accept"): pass_control(buddy)
	else:
		pass
	
	velocity = apply_gravity(velocity)
	update_animation()
	move_and_slide()


func _ready():
	woosh_time.one_shot = true
	add_child(woosh_time)
	add_child(pass_control_buffer)
	woosh_time.timeout.connect(woosh_time_over)
	pass_control_buffer.one_shot = true
	cam.enabled = currently_controlled
	
