extends Character
class_name PlayerCharacter

const TERMINAL_VELOCITY = 300

signal swapped_out
signal swapped_in

var currently_controlled = false
var face_right = true

@export var buddy: PlayerCharacter
@onready var woosh_time = Timer.new()
@onready var pass_control_buffer = Timer.new()

# Animation enum
enum Anim {IDLE, RUN, AIR_UP, AIR_DOWN}

func move_by_input(v: Vector2) -> Vector2:
	var vn = v
	vn.x = 0
	
	if Input.is_action_pressed("ui_left"):
		vn.x -= 1
		anim = "run"
	if Input.is_action_pressed("ui_right"):
		vn.x += 1
		anim = "run"
	
	if Input.is_action_just_pressed("ui_right"):
		face_right = true
		$sprite.flip_h = !face_right
	elif Input.is_action_just_pressed("ui_left"):
		face_right = false
		$sprite.flip_h = !face_right
	
	if can_jump(): vn = jump(vn)

	vn.x *= 200
	return vn


func can_jump() -> bool:
	return Input.is_action_just_pressed("ui_up") and is_on_floor()


func jump(v: Vector2) -> Vector2:
	# WOOSH time
	grav_excempt = true
	woosh_time.wait_time = 0.1
	woosh_time.start()
	
	$anim.play("jump")
	
	var vn = v
	vn.y = -250
	return vn
	

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
		anim = "air_down"
	elif velocity.y < 0:
		anim = "air_up"
	elif velocity == Vector2.ZERO:
		anim = anim_base
	
	return vn


func pass_control(to: PlayerCharacter):
	# Just a safeguard:
	if !currently_controlled: return
	currently_controlled = false

	# Start the timer
	pass_control_buffer.wait_time = 0.1
	pass_control_buffer.timeout.connect(to.recieve_control.bind(self))
	pass_control_buffer.start()

	# Send the signals
	swapped_out.emit()
	to.swapped_in.emit()


func recieve_control(from: PlayerCharacter):
	currently_controlled = true
	from.pass_control_buffer.timeout.disconnect(recieve_control)


func on_targeted():
	GoConductor.get_stage("music").CurrentlyPlaying.CueInIdx(1)


func on_targeted_stopped():
	print(GoConductor.get_stage("music").CurrentlyPlaying.name)
	GoConductor.get_stage("music").CurrentlyPlaying.CueOutIdx(1)


func _process(delta):
	if currently_controlled:
		velocity = move_by_input(velocity)
		if Input.is_action_just_pressed("ui_accept"): pass_control(buddy)
	else:
		velocity.x = 0
	
	velocity = apply_gravity(velocity)
	update_animation()
	move_and_slide()


func _ready():
	anim_base = "idle"
	woosh_time.one_shot = true
	add_child(woosh_time)
	add_child(pass_control_buffer)
	woosh_time.timeout.connect(woosh_time_over)
	pass_control_buffer.one_shot = true
	
