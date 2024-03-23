extends Character

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


func _on_detection_body_exited(body:Node2D):
	if body == target:
		target = null


func _ready():
	anim_base = "default"
	home = position
