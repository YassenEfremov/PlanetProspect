extends Area3D


var last_press_pos

signal star_selected


func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventScreenTouch:
		if event.pressed:
			last_press_pos = event.position
			
		if not event.pressed and event.position == last_press_pos:
			$"../".select_star(self)


func _on_select_button_pressed():
	star_selected.emit()
