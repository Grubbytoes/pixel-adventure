extends CharacterBody2D
class_name Character

var grav_excempt = false
var grav_v = 0.0
var anim: String
var anim_base: String

@onready var sprite = $sprite

func update_animation():
	sprite.play(anim)