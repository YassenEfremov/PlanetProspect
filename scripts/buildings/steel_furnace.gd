class_name SteelFurnace extends Building


var generated_steel: int
var steel_generation_rate: int = 1
var steel_generation_interval: int = 2
var steel_capacity: int = 50


func _ready():
	$GenerationTimer.wait_time = steel_generation_interval
	action_buttons.push_back(get_node("/root/Main/SafeArea/UI/MainView/Actions/RemoveBuildingButton"))
	$Resources/Value.text = str(generated_steel)


func _on_construction_finished():
	$MeshInstance3D.show()
	$steel_furnace.show()
	$Resources.show()
	action_buttons.push_back(get_node("/root/Main/SafeArea/UI/MainView/Actions/CollectButton"))
	$GenerationTimer.start()


func _on_generation_timer_timeout():
	if generated_steel < steel_capacity:
		generated_steel += steel_generation_rate
		$Resources/Value.text = str(generated_steel)
