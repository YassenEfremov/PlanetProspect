extends Area3D


var launched: bool = false


func _ready():
	pass # Replace with function body.


func _process(delta):
	if launched:
		position += basis.y * delta * 2


func launch():
	reparent($"/root/Main")
	launched = true
