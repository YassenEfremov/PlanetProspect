class_name Pad extends Building


var rocket = null


func _ready():
	action_buttons.push_back(get_node("/root/Main/SafeArea/UI/MainView/Actions/RemoveBuildingButton"))


func launch_rocket():
	rocket.launch()
	rocket = null


func _on_construction_finished():
	$MeshInstance3D.show()
	action_buttons.push_back(get_node("/root/Main/SafeArea/UI/MainView/Actions/AddRocketButton"))
	action_buttons.push_back(get_node("/root/Main/SafeArea/UI/MainView/Actions/DestinationButton"))
	action_buttons.push_back(get_node("/root/Main/SafeArea/UI/MainView/Actions/LaunchButton"))
