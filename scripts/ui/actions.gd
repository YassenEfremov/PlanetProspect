extends HBoxContainer


signal max_resources_reached


func _on_remove_building_button_pressed():
	$"../../".selected_planet.remove_building()


func _on_add_rocket_button_pressed():
	if not $"../../".selected_planet.selected_building.rocket:
		$"../../".selected_planet.selected_building.rocket = load("res://scenes/rocket.tscn").instantiate()
		$"../../".selected_planet.selected_building.rocket.position += Vector3.UP * 1
		$"../../".selected_planet.selected_building.add_child($"../../".selected_planet.selected_building.rocket)
		$"/root/Main/UI/MainView/Actions/AddRocketButton".disabled = true
		if $"/root/Main/UI/StarMapView".selected_destination:
			$"/root/Main/UI/MainView/Actions/LaunchButton".disabled = false


func _on_destination_button_pressed():
	$"../../StarMapView".choosing_destination = true
	$"../../"._on_star_map_button_pressed()
	$"../../Message".text = "Select a destination"


func _on_launch_button_pressed():
	$"/root/Main/UI/TripView".start_trip()
	$"../../".selected_planet.selected_building.launch_rocket()
	$"/root/Main/UI/MainView/Actions/AddRocketButton".disabled = false
	$"/root/Main/UI/MainView/Actions/DestinationButton".disabled = false
	$"/root/Main/UI/MainView/Actions/LaunchButton".disabled = true


func _on_collect_button_pressed():
	if $"../../".selected_planet.selected_building.get("generated_energy") != null \
		and $"../../Resources".can_store_energy($"../../".selected_planet.selected_building.generated_energy):
		$"../../Resources".add_energy($"../../".selected_planet.selected_building.generated_energy)
		$"../../".selected_planet.selected_building.generated_energy = 0
		$"../../".selected_planet.selected_building.get_node("Label3D").text = str(0)

	elif $"../../".selected_planet.selected_building.get("generated_steel") != null \
		and $"../../Resources".can_store_steel($"../../".selected_planet.selected_building.generated_steel):
		$"../../Resources".add_steel($"../../".selected_planet.selected_building.generated_steel)
		$"../../".selected_planet.selected_building.generated_steel = 0
		$"../../".selected_planet.selected_building.get_node("Label3D").text = str(0)

	else:
		max_resources_reached.emit()
