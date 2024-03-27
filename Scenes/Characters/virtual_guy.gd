extends PlayerCharacter


func _physics_process(delta):
	super._physics_process(delta)
	for i in get_slide_collision_count():
		var o = get_slide_collision(i).get_collider()
		if o.has_method("be_pushed"):
			o.be_pushed(face_right)


func on_targeted():
	print(GoConductor.get_stage("music").CurrentlyPlaying.name)
	GoConductor.get_stage("music").CurrentlyPlaying.CueInName("attacked")


func on_targeted_stopped():
	GoConductor.get_stage("music").CurrentlyPlaying.CueOutName("attacked")
