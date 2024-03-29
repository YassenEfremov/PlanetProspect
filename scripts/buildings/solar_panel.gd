class_name SolarPanel extends Building


var generated_energy: int
var energy_generation_rate: int = 1
var energy_generation_interval: int = 2
var energy_capacity: int = 20


func _ready():
	$GenerationTimer.wait_time = energy_generation_interval
	if not permanent:
		action_buttons.push_back($"/root/Main/SafeArea/UI/MainView/Actions/RemoveBuildingButton")
		$Construction.start()
	else:
		$Construction._on_build_timer_timeout()
	$Resources/Value.text = str(generated_energy)


func _on_construction_finished():
	$solar_panel.show()
	$Resources.show()
	action_buttons.push_back($"/root/Main/SafeArea/UI/MainView/Actions/CollectButton")
	$GenerationTimer.start()


func _on_generation_timer_timeout():
	if generated_energy < energy_capacity:
		generated_energy += energy_generation_rate
		$Resources/Value.text = str(generated_energy)
