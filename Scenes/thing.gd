extends Node2D

@onready var camera = $camera

var player_character: Character


func to_frog():
	player_character = $ninja_frog
	GoConductor.get_stage("music").Cue("vr")
	

func to_vr_guy():
	player_character = $virtual_guy
	GoConductor.get_stage("music").Cue("frog")


func _ready():
	$virtual_guy.currently_controlled = true
	GoConductor.load_stage("mystage", "music")
	to_vr_guy()
	GoConductor.get_stage("music").Play()


func _process(_delta):
	camera.position = player_character.position
