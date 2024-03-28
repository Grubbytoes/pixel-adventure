extends Node2D


@onready var title_screen = $title_screen
@onready var hud = $hud
@onready var level_layer = $level_layer

var current_level: PackedScene


func title_screen_play_pressed():
	print("ping")
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


func _ready():
	title_screen.visible = true
	hud.visible = false
