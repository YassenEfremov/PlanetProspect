class_name SteelFurnace extends Building


var generated_steel: int
var steel_generation_rate: int = 1
var steel_generation_interval: int = 2
var steel_capacity: int = 50


func _ready():
	$Timer.wait_time = steel_generation_interval
	action_buttons.push_back(get_node("/root/Main/SafeArea/UI/MainView/Actions/RemoveBuildingButton"))
	action_buttons.push_back(get_node("/root/Main/SafeArea/UI/MainView/Actions/CollectButton"))
	$Label3D.text = str(generated_steel)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_timer_timeout():
	if generated_steel < steel_capacity:
		generated_steel += steel_generation_rate
		$Label3D.text = str(generated_steel)
