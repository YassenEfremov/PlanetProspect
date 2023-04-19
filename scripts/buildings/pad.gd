class_name Pad extends Building


var rocket = null
var launched: bool = false


func _ready():
	action_buttons.push_back(get_node("/root/Main/UI/MainView/Actions/RemoveBuildingButton"))
	action_buttons.push_back(get_node("/root/Main/UI/MainView/Actions/AddRocketButton"))
#	action_buttons.push_back(get_node("/root/Main/UI/MainView/Actions/DestinationButton"))
	action_buttons.push_back(get_node("/root/Main/UI/MainView/Actions/LaunchButton"))


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if launched:
		rocket.position += Vector3.UP * delta
