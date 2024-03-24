extends Character

signal targeting
signal stopped_targeting

var target: PlayerCharacter
var home: Vector2

func persue(point: Vector2, speed_factor = 1.0) -> Vector2:
	var vn = point - position
	vn = vn.normalized() * 100 * speed_factor
	return vn


func _physics_process(_delta):
	if target != null:
		velocity = persue(target.position)
	elif (position - home).length() > 10:
		velocity = persue(home, 0.5)
	else:
		velocity = Vector2.ZERO

	move_and_slide()
	update_animation()


func _on_detection_body_entered(body:Node2D):
	if body is PlayerCharacter:
		target = body

		# Signals
		if body.has_method("on_targeted"):
			targeting.connect(body.on_targeted)
			targeting.emit()
			targeting.disconnect(body.on_targeted)


func _on_detection_body_exited(body:Node2D):
	if body == target:
		target = null

		# Signals
		if body.has_method("on_targeted_stopped"):
			stopped_targeting.connect(body.on_targeted_stopped)
			stopped_targeting.emit()
			stopped_targeting.disconnect(body.on_targeted_stopped)


func _ready():
	anim_base = "default"
	home = position
