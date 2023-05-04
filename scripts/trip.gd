class_name Trip extends Panel


var destination

signal trip_completed


func _process(delta):
	if $ProgressBar.value < $ProgressBar.max_value:
		$ProgressBar.value += 0.05
	else:
		trip_completed.emit()
		$VisitButton.show()


func _on_visit_button_pressed():
	if get_node("/root/Main/UI").selected_planet.selected_building:
		get_node("/root/Main/UI").selected_planet.click_building(get_node("/root/Main/UI").selected_planet.selected_building)
	get_node("/root/Main/UI").selected_planet.hide()
	get_node("/root/Main/UI").selected_planet.get_node("UI").hide()
	get_node("/root/Main/UI").select_planet(destination)
	get_node("/root/Main/UI").selected_planet.show()
	get_node("/root/Main/UI").selected_planet.get_node("UI").show()
	destination.get_node("Camera3D").make_current()
	get_node("/root/Main/UI")._on_back_button_pressed()
	$"../../../".end_trip(self)
