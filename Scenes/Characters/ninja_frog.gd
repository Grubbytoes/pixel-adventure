extends PlayerCharacter

var double_jump = true


func can_jump() -> bool:
	if !Input.is_action_just_pressed("ui_up"):
		pass
	elif is_on_floor():
		double_jump = true
		return true
	elif double_jump:
		double_jump = false
		return true
	return false
