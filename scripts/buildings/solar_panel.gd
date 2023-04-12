class_name SolarPanel extends Building


var generated_energy: int
var energy_generation_rate: int = 1
var energy_generation_interval: int = 2
var energy_capacity: int = 20


func _ready():
	$Timer.wait_time = energy_generation_interval
	action_buttons.push_back(get_node("/root/Main/UI/MainView/Actions/RemoveBuildingButton"))
	action_buttons.push_back(get_node("/root/Main/UI/MainView/Actions/CollectButton"))
	$Label3D.text = str(generated_energy)


func _on_timer_timeout():
	if generated_energy < energy_capacity:
		generated_energy += energy_generation_rate
		$Label3D.text = str(generated_energy)
