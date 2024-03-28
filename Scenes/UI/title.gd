extends CanvasLayer

signal play_pressed


func emmit_play_pressed():
	print("PLay button pressed internal")
	play_pressed.emit()
