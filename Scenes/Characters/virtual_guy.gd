extends PlayerCharacter


func _process(delta):
	super._process(delta)
	for i in get_slide_collision_count():
		var o = get_slide_collision(i).get_collider()
		if o.has_method("be_pushed"):
			o.be_pushed(face_right)


func on_targeted():
	pass


func on_targeted_stopped():
	pass