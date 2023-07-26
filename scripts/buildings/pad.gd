class_name Pad extends Building


var rocket = null


func _ready():
	if not permanent:
		action_buttons.push_back($"/root/Main/SafeArea/UI/MainView/Actions/RemoveBuildingButton")
		$Construction.start()
	else:
		$Construction._on_build_timer_timeout()


func launch_rocket():
	rocket.launch()
	rocket = null


func _on_construction_finished():
	$pad.show()
	action_buttons.push_back($"/root/Main/SafeArea/UI/MainView/Actions/AddRocketButton")
	action_buttons.push_back($"/root/Main/SafeArea/UI/MainView/Actions/DestinationButton")
	action_buttons.push_back($"/root/Main/SafeArea/UI/MainView/Actions/LaunchButton")
