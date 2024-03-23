extends Node2D
class_name BaseStage

enum ReferenceChildren {BY_NAME, BY_IDX}

@export var ref = ReferenceChildren.BY_NAME
var pointer # Quack quack, motherfucker


func _ready():
	# There are three option for what 'pointer' will actually do. As GDScript is dynamicly typed, we can do this!
	# A pointer to one GoConductor node, this is defualted to if there is only one child node
	# A dictionary where each node is mapped by it's name
	# An array for each node in order of their heigherarchy
	var child_count = get_child_count()

	if child_count > 1:
		pointer = Dictionary()
		for i in range(child_count):
			get_children()
	elif child_count == 1:
		pointer = get_child(0)
	else:
		push_warning(name + " instanced as an empty stage")


func _pointer_dictionary() -> Dictionary:
	var dict = Dictionary()
	
	for child in get_children():
		dict[child.name] = child

	return dict


func _pointer_array(n) -> Array:
	var array = []

	for i in n:
		array.append(get_child(i))
	
	return array
