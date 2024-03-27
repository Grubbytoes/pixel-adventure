extends Node2D

@onready var _stages: Dictionary = {}
@onready var _settings: Dictionary = {}

func say_hello():
	print("Hello, from GoConductor!")


func get_stage(stage_name) -> Node:
	return _stages.get(stage_name).pointer


## Loads the stage of the given name from the stages directory. Returns true if successful
## Path does NOT need to include '.tsct' or the root stages directory
func load_stage(stage_name: String, pseudonym:String="") -> bool:
	# Setting up path and name/pseudonym
	var path = _to_path(stage_name)
	if pseudonym != "": stage_name = pseudonym

	# Check if a stage with that name is already loaded
	if _stages.has(stage_name):
		printerr("GoConductor: Tried to load a stage of stage_name '"+stage_name+"' when one is already loaded")
		return false
	
	# Add the stage
	var newstage = _instance_stage(path)
	add_child(newstage)
	_stages[stage_name] = newstage
	return true


## Finds the stage of the given name and removes it, return true if found and successfully removed
func unstage(stage_name: String) -> bool:
	var stage = _stages.get(stage_name)
	if stage != null: stage.queue_free()
	return _stages.erase(stage_name)


func swap_stage(new_stage_name: String, stage_name: String) -> bool:
	if !_stages.has(stage_name):
		return false
	
	# Assigning variables
	var old_stage = _stages[stage_name]
	var new_stage = _instance_stage(_to_path(new_stage_name))

	# Swapping over the nodes
	add_child(new_stage)
	_stages[stage_name] = new_stage
	old_stage.queue_free()

	# Done!
	return true


func mute_music(mute=true):
	AudioServer.set_bus_mute(get_music_bus_idx(), mute)


func mute_sfx(mute=true):
	AudioServer.set_bus_mute(get_sfx_bus_idx(), mute)


func load_settings():
	var settings_raw = FileAccess.open("GoConductor/settings.txt", FileAccess.READ).get_as_text()

	for line in settings_raw.split("\n"):
		if !line: break
		var kv = line.replace(" ", "").split(":")
		_settings[kv[0]] = kv[1]


func _ready():
	load_settings()
	

func _to_path(somestr: String) -> String:
	return "res://GoConductor/Stages/" + somestr + ".tscn"


func _instance_stage(path: String) -> Node:
	var s = load(path)
	return s.instantiate()


func get_music_bus_idx() -> int:
	var idx = AudioServer.get_bus_index(_settings["musicBus"])
	print(idx)
	return idx


func get_sfx_bus_idx() -> int:
	return AudioServer.get_bus_index(_settings["soundEffectsBus"])



