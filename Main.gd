extends Node2D


@onready var title_screen = $title_screen
@onready var hud = $hud
@onready var level_layer = $level_layer

var current_level: PackedScene


func title_screen_play_pressed():
	# Swap the UI
	title_screen.visible = false
	hud.visible = true

	# Instance the level
	if current_level != null: level_layer.add_child(current_level.instantiate())


func return_to_title():
	print("pong")

	# Swap the UI
	title_screen.visible = true
	hud.visible = false

	# Clear the level layer
	for l in level_layer.get_children(): l.queue_free()


func _ready():
	current_level = load("res://Scenes/Levels/thing.tscn")
