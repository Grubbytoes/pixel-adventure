extends Node2D

@onready var camera = $camera
@onready var music = GoConductor.get_stage_pointer('music')

var player_character: PlayerCharacter


func to_frog():
	player_character = $ninja_frog
	music.CueName('ebm')
	

func to_vr_guy():
	player_character = $virtual_guy
	music.CueName('tangerine')


func _ready():
	to_vr_guy()
	player_character.currently_controlled = true


func _process(_delta):
	camera.position = player_character.position
	
