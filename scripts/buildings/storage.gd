class_name Storage extends Building


func _ready():
	action_buttons.push_back(get_node("/root/Main/SafeArea/UI/MainView/Actions/RemoveBuildingButton"))


func _on_construction_finished():
	$MeshInstance3D.show()
