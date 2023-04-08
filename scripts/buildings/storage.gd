extends "../building.gd"


func _ready():
	action_buttons.push_back(get_node("/root/Main/UI/MainView/Actions/RemoveBuildingButton"))
