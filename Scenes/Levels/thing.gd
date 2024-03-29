extends Node2D

@onready var camera = $camera

var player_character: PlayerCharacter


func to_frog():
	player_character = $ninja_frog
	

func to_vr_guy():
	player_character = $virtual_guy


func _ready():
	player_character = $virtual_guy
	player_character.currently_controlled = true


func _process(_delta):
	camera.position = player_character.position
	
