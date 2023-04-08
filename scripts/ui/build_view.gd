extends ColorRect


signal building_selected(building_name: String)
signal not_enough_resources
signal max_buildings_reached


func _on_steel_furnace_button_pressed():
	if not $"../Resources".enough_steel(100):
		not_enough_resources.emit()

	elif $"/root/Main/UI".selected_planet.buildings.size() == $"/root/Main/UI".selected_planet.max_buildings:
		max_buildings_reached.emit()

	else:
		building_selected.emit("steel_furnace")


func _on_solar_panel_button_pressed():
	if not $"../Resources".enough_steel(50):
		not_enough_resources.emit()

	elif $"/root/Main/UI".selected_planet.buildings.size() == $"/root/Main/UI".selected_planet.max_buildings:
		max_buildings_reached.emit()

	else:
		building_selected.emit("solar_panel")


func _on_storage_button_pressed():
	if not $"../Resources".enough_steel(400):
		not_enough_resources.emit()

	elif $"/root/Main/UI".selected_planet.buildings.size() == $"/root/Main/UI".selected_planet.max_buildings:
		max_buildings_reached.emit()

	else:
		building_selected.emit("storage")


func _on_pad_button_pressed():
	if not $"../Resources".enough_steel(300):
		not_enough_resources.emit()

	elif $"/root/Main/UI".selected_planet.buildings.size() == $"/root/Main/UI".selected_planet.max_buildings:
		max_buildings_reached.emit()

	else:
		building_selected.emit("pad")
