extends CanvasLayer

signal on_pause(paused: bool) 
signal on_mute(mute: bool)
signal on_quit

@onready var filter: Control = $filter

# If true will, handel muting music internally. If flase, on_mute will still be emmited, but will bypass interfacing with GoConductor.
@export var handle_mute = true
@export var handle_pause = true

func mute_pressed(mute: bool):
	on_mute.emit(mute)


func pause_pressed(paused: bool):
	if handle_pause: get_tree().paused = paused
	filter.visible = paused
	on_pause.emit(paused)


func quit_pressed():
	on_quit.emit()
	get_tree().paused = false
	filter.visible = false
