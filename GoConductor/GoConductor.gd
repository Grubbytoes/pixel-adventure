extends Node2D

@onready var _stages: Dictionary = Dictionary()

func say_hello():
    print("Hello, from GoConductor!")


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

    # load the stage, and make sure there's no error
    var newstage = load(path).instantiate()
    if newstage is Error:
        printerr("GoCOnductor: there was an error when loading stage '"+stage_name+"'")
        return false
    
    # Add the stage
    add_child(newstage)
    _stages[stage_name] = newstage
    return true


## Finds the stage of the given name and removes it, return true if found and successfully removed
func unstage(stage_name: String) -> bool:
    var stage = _stages.get(stage_name)
    if stage != null: stage.queue_free()
    return _stages.erase(stage_name)


func swap_stage(new_stage_name: String, stage_name: String) -> bool:
    return false


func _to_path(somestr: String) -> String:
    return "res://GoConductor/Stages/" + somestr + ".tscn"