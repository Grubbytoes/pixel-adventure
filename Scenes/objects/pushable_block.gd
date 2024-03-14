extends RigidBody2D


func be_pushed(push_right: bool):
	if push_right:
		move_and_collide(Vector2.RIGHT)
	else:
		move_and_collide(Vector2.LEFT)
