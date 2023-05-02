class_name Building extends Area3D


@export var steel_cost: int

var action_buttons: Array[Button]
var last_press_pos: Vector2


func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventScreenTouch:
		if event.pressed:
			last_press_pos = event.position
			
		if not event.pressed and event.position == last_press_pos:
			$"../../".select_building(self)
