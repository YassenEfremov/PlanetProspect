class_name Trip extends Panel


var destination

signal trip_completed


#func _init(destination):
#	self.destination = destination


func _process(delta):
	if $ProgressBar.value < $ProgressBar.max_value:
		$ProgressBar.value += 0.05
	else:
		trip_completed.emit()
		$VisitButton.show()


func _on_visit_button_pressed():
#	$"/root/Main/Camera3D".focused_planet = destination
	pass
