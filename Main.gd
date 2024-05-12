extends Node2D


@onready var title_screen = $title_screen
@onready var hud = $hud
@onready var level_layer = $level_layer
@onready var music_idx = GoConductor.get_music_bus_idx()

var current_level: PackedScene
var music

func title_screen_play_pressed():
	# Swap the UI
	title_screen.visible = false
	hud.visible = true

	# Instance the level
	current_level = load("res://Scenes/Levels/thing.tscn")
	level_layer.add_child(current_level.instantiate())
	level_layer.visible = true


func return_to_title():
	# Swap the UI
	title_screen.visible = true
	hud.visible = false

	# Clear the level layer
	level_layer.visible = false
	for l in level_layer.get_children(): l.queue_free()
	music.CueName('mackster')

	# Remove paused effect
	pause_effect(false)


func _ready():
	title_screen.visible = true
	hud.visible = false
	GoConductor.load_stage("my_music", "music")
	music = GoConductor.get_stage_pointer("music")
	music.CueName("mackster")
	music.Play()


func mute_music(muted: bool):
	GoConductor.mute_music(muted)


func pause_effect(paused: bool):
	if paused:
		AudioServer.add_bus_effect(music_idx, AudioEffectBandPassFilter.new())
	else:
		AudioServer.remove_bus_effect(music_idx, 0)