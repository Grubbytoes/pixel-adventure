extends CanvasLayer

signal on_pause(paused: bool) 
signal on_mute(mute: bool)
signal on_quit

@onready var filter: Control = $filter

# If true will, handel muting music internally. If flase, on_mute will still be emmited, but will bypass interfacing with GoConductor.
@export var handle_mute = true
@export var handle_pause = true

func mute_pressed(mute: bool):
	if handle_mute: GoConductor.mute_music(mute)
	on_mute.emit(mute)


func pause_pressed(paused: bool):
	if handle_pause: get_tree().paused = paused
	filter.visible = paused
	on_pause.emit(paused)

	# The audio effect
	if paused:
		var effect = AudioEffectLowPassFilter.new()
		effect.cutoff_hz = 1200
		effect.resonance = 0.7
		AudioServer.add_bus_effect(GoConductor.get_music_bus_idx(), effect, 0)
	else:
		AudioServer.remove_bus_effect(GoConductor.get_music_bus_idx(), 0)


func quit_pressed():
	on_mute.emit()
