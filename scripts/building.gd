extends Area3D


var action_buttons: Array[Button]
@export var steel_cost: int
var last_press_pos


func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventScreenTouch:
		if event.pressed:
			last_press_pos = event.position
			
		if not event.pressed and event.position == last_press_pos:
			$"../../".select_building(self)
