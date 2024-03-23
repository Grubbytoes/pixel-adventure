extends Node2D

@onready var camera = $camera

var player_character: PlayerCharacter


func to_frog():
	player_character = $ninja_frog
	var b = GoConductor.get_stage("music").CueName("vr")
	

func to_vr_guy():
	player_character = $virtual_guy
	var b = GoConductor.get_stage("music").CueName("frog")


func _ready():
	$virtual_guy.currently_controlled = true
	GoConductor.load_stage("mystage", "music")
	to_vr_guy()
	GoConductor.get_stage("music").Play()


func _process(_delta):
	camera.position = player_character.position
