extends Control


var max_energy: int = 1000
var max_steel: int = 1000
var total_energy: int = 800
var total_steel: int = 900


func _ready():
	update_energy_indicator()
	update_steel_indicator()


# Energy

func enough_energy(energy: int) -> bool:
	return total_energy - energy >= $EnergyBar.min_value


func can_store_energy(energy: int) -> bool:
	return total_energy + energy <= $EnergyBar.max_value


func add_energy(energy: int):
	if total_energy + energy <= $EnergyBar.max_value:
		total_energy += energy
		update_energy_indicator()


func take_energy(energy: int):
	if total_energy - energy >= $EnergyBar.min_value:
		total_energy -= energy
		update_energy_indicator()


func update_energy_indicator():
	$EnergyBar.max_value = max_energy
	$EnergyBar.value = total_energy
	$EnergyBar/Value.text = str(total_energy)


# Steel

func enough_steel(steel: int) -> bool:
	return total_steel - steel >= $SteelBar.min_value


func can_store_steel(steel: int) -> bool:
	return total_steel + steel <= $SteelBar.max_value


func add_steel(steel: int):
	if total_steel + steel <= $SteelBar.max_value:
		total_steel += steel
		update_steel_indicator()


func take_steel(steel: int):
	if total_steel - steel >= $SteelBar.min_value:
		total_steel -= steel
		update_steel_indicator()


func update_steel_indicator():
	$SteelBar.max_value = max_steel
	$SteelBar.value = total_steel
	$SteelBar/Value.text = str(total_steel)


func _on_building_placed(building: Building):
	take_steel(building.steel_cost)
	if building is Storage:
		max_steel += 1000
	
	update_steel_indicator()
