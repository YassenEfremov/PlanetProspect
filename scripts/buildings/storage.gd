class_name Storage extends Building


func _ready():
	if not permanent:
		action_buttons.push_back($"/root/Main/SafeArea/UI/MainView/Actions/RemoveBuildingButton")
		$Construction.start()
	else:
		$Construction._on_build_timer_timeout()


func _on_construction_finished():
	$storage.show()
