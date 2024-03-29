extends Node2D

@onready var camera = $camera

var player_character: PlayerCharacter


func to_frog():
	player_character = $ninja_frog
	GoConductor.get_stage("music").CueName("frog")
	

func to_vr_guy():
	player_character = $virtual_guy
	GoConductor.get_stage("music").CueName("vr")


func _ready():
	$virtual_guy.currently_controlled = true
	to_vr_guy()
	GoConductor.get_stage("music").Play()


func _process(_delta):
	camera.position = player_character.position
