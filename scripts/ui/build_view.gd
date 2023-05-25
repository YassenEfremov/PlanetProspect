extends Control


signal building_selected
signal not_enough_resources
signal max_buildings_reached


func _on_steel_furnace_button_pressed():
	if not $"../Resources".enough_steel(100):
		not_enough_resources.emit()

	elif $"/root/Main/SafeArea/UI".selected_planet.buildings.size() == $"/root/Main/SafeArea/UI".selected_planet.max_buildings:
		max_buildings_reached.emit()

	else:
		$"../".selected_planet.building_to_place = load("res://scenes/buildings/steel_furnace.tscn")
		building_selected.emit()


func _on_solar_panel_button_pressed():
	if not $"../Resources".enough_steel(50):
		not_enough_resources.emit()

	elif $"/root/Main/SafeArea/UI".selected_planet.buildings.size() == $"/root/Main/SafeArea/UI".selected_planet.max_buildings:
		max_buildings_reached.emit()

	else:
		$"../".selected_planet.building_to_place = load("res://scenes/buildings/solar_panel.tscn")
		building_selected.emit()


func _on_storage_button_pressed():
	if not $"../Resources".enough_steel(400):
		not_enough_resources.emit()

	elif $"/root/Main/SafeArea/UI".selected_planet.buildings.size() == $"/root/Main/SafeArea/UI".selected_planet.max_buildings:
		max_buildings_reached.emit()

	else:
		$"../".selected_planet.building_to_place = load("res://scenes/buildings/storage.tscn")
		building_selected.emit()


func _on_pad_button_pressed():
	if not $"../Resources".enough_steel(300):
		not_enough_resources.emit()

	elif $"/root/Main/SafeArea/UI".selected_planet.buildings.size() == $"/root/Main/SafeArea/UI".selected_planet.max_buildings:
		max_buildings_reached.emit()

	else:
		$"../".selected_planet.building_to_place = load("res://scenes/buildings/pad.tscn")
		building_selected.emit()
