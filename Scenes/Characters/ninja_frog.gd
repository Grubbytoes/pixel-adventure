extends Character

var double_jump = true


func can_jump() -> bool:
	if !Input.is_action_just_pressed("ui_up"):
		pass
	elif is_on_floor():
		double_jump = true
		return true
	elif double_jump:
		do_double_jump()
		return false
	return false


func do_double_jump():
	double_jump = false
	velocity = jump(velocity)
	$anim.play("double_jump")


func _process(delta):
	super._process(delta)
