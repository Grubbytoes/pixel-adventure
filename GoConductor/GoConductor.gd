extends Node2D

@onready var _stages: Dictionary = Dictionary()

func say_hello():
    print("Hello, from GoConductor!")


## Loads the stage of the given name from the stages directory. Returns true if successful
## Path does NOT need to include '.tsct' or the root stages directory
func load_stage(path: String) -> bool:
    # Check if a stage with that name is already loaded
    if _stages.has(path):
        printerr("GoConductor: Tried to load a stage of name '"+path+"' when one is already loaded")
        return false

    # load the stage, and make sure there's no error
    var newstage = load(_to_path(path)).instantiate()
    if newstage is Error:
        printerr("GoCOnductor: there was an error when loading stage '"+path+"'")
        return false
    
    # Add the stage
    add_child(newstage)
    _stages[path] = newstage
    return true


## Finds the stage of the given name and removes it, return true if found and successfully removed
func unstage(stage_name: String) -> bool:
    var stage = _stages.get(stage_name)
    if stage != null: stage.queue_free()
    return _stages.erase(stage_name)


func _to_path(somestr: String) -> String:
    return "res://GoConductor/Stages/" + somestr + ".tscn"