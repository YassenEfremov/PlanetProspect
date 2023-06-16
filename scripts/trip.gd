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
	if $"/root/Main/SafeArea/UI".selected_planet.selected_building:
		$"/root/Main/SafeArea/UI".selected_planet.click_building($"/root/Main/SafeArea/UI".selected_planet.selected_building)
	$"/root/Main/SafeArea/UI".selected_planet.hide()
	$"/root/Main/SafeArea/UI".selected_planet.get_node("UI").hide()
	$"/root/Main/SafeArea/UI".select_planet(destination)
	$"/root/Main/SafeArea/UI".selected_planet.show()
	$"/root/Main/SafeArea/UI".selected_planet.get_node("UI").show()
	destination.get_node("Camera3D").make_current()
	$"/root/Main/SafeArea/UI"._on_back_button_pressed()
	$"../../../".end_trip(self)
