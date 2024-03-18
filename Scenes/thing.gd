extends Node2D

@onready var camera = $camera

var player_character: Character


func to_frog():
	player_character = $ninja_frog
	GoConductor.unstage("mystage")
	

func to_vr_guy():
	player_character = $virtual_guy
	GoConductor.load_stage("mystage")


func _ready():
	$virtual_guy.currently_controlled = true
	to_vr_guy()


func _process(_delta):
	camera.position = player_character.position
