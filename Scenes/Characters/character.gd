extends CharacterBody2D
class_name Character

const TERMINAL_VELOCITY = 300

@export var speed = 12
var face_right = true
var grav_excempt = false
var currently_controlled = true
var grav_v = 0.0
var anim: Anim
@onready var sprite = $sprite
@onready var woosh_time = Timer.new()

# Animation enum
enum Anim {IDLE, RUN, AIR_UP, AIR_DOWN}

func move_by_input(v: Vector2) -> Vector2:
	var vn = v
	var idle = true
	vn.x = 0
	
	if Input.is_action_pressed("ui_left"):
		vn.x -= 1
		idle = false
		anim = Anim.RUN
	if Input.is_action_pressed("ui_right"):
		vn.x += 1
		idle = false
		anim = Anim.RUN
	
	if Input.is_action_just_pressed("ui_right"):
		face_right = true
		$sprite.flip_h = !face_right
	elif Input.is_action_just_pressed("ui_left"):
		face_right = false
		$sprite.flip_h = !face_right
	
	if Input.is_action_just_pressed("ui_up") and is_on_floor():
		vn = jump(vn)
	
	if idle:
		anim = Anim.IDLE
	
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
	
	return vn
		

func update_animation():
	match anim:
		Anim.IDLE:
			sprite.play("idle")
		Anim.RUN:
			sprite.play("run")
		Anim.AIR_DOWN:
			sprite.play("air_down")
	

func _process(delta):
	velocity =  move_by_input(velocity)
	velocity = apply_gravity(velocity)
	update_animation()

	move_and_slide()


func _ready():
	woosh_time.one_shot = true
	add_child(woosh_time)
	woosh_time.timeout.connect(woosh_time_over)
	
