extends Control


var selected_planet: Planet = null
var current_view


func _ready():
	current_view = $MainView
	select_planet($"../../SolarSystem/Earth")


func select_planet(planet):
	if !selected_planet:
		# Newly selected planet
#		var actions = planet.get_node("Actions")
#		if actions.get_parent():
#			actions.get_parent().remove_child(actions)
#		get_node("/root/Main/SafeArea/UI").add_child(actions)
		
		selected_planet = planet
		planet.get_node("%Outline").visible = true
		planet.get_node("Axis").visible = true
#		actions.show()
#		get_node("/root/Main/SafeArea/UI/MainView/Actions").show()
	
	else:
		if planet != selected_planet:
			# Change selected planet
#			get_node("/root/Main/SafeArea/UI").remove_child(selected_planet.get_node("Actions"))

			selected_planet.get_node("%Outline").visible = false
			selected_planet.get_node("Axis").visible = false
			selected_planet = planet
			planet.get_node("%Outline").visible = true
			planet.get_node("Axis").visible = true
		
		else:
			# Deselect current planet
#			var actions = get_node("/root/Main/SafeArea/UI/Actions")
#			if actions.get_parent():
#				actions.get_parent().remove_child(actions)
#			planet.add_child(actions)
			
			selected_planet = null
			planet.get_node("%Outline").visible = false
			planet.get_node("Axis").visible = false
#			actions.hide()
#			get_node("/root/Main/SafeArea/UI/MainView/Actions").hide()


func _on_star_map_button_pressed():
	current_view = $StarMapView
	$"../../SolarSystem".hide()
	$"../../OmniLight3D".hide()
	$MainView.hide()
	$Resources.hide()
	selected_planet.get_node("UI/BuildingsLabel").hide()

	$"../../StarMap/Camera3D".make_current()
	$"../../StarMap".orient()
	$"../../StarMap".show()
	$StarMapView.show()
	$BackButton.show()


func _on_build_button_pressed():
	current_view = $BuildView
	
	$MainView.hide()
	selected_planet.get_node("UI/BuildingsLabel").hide()
	
	$BuildView.show()
	$BackButton.show()


func _on_trips_button_pressed():
	current_view = $TripView
	
	$MainView.hide()
	selected_planet.get_node("UI/BuildingsLabel").hide()
	
	$TripView.show()
	$BackButton.show()


func _on_build_view_building_selected():
	if selected_planet.selected_building:
		selected_planet.deselect_building(selected_planet.selected_building)
	_on_back_button_pressed()
	$Message.text = "Click somewhere on the planet to place the building"
	$MainView/CancelButton.show()


func _on_cancel_button_pressed():
	$Message.text = ""
	$MainView/CancelButton.hide()


func _on_build_view_max_buildings_reached():
	_on_back_button_pressed()
	$Message.text = "Maximum number of buildings reached!"
	await get_tree().create_timer(2.0).timeout
	$Message.text = ""


func _on_build_view_not_enough_resources():
	$Message.text = "Not enough resources!"
	await get_tree().create_timer(2.0).timeout
	$Message.text = ""


func _on_actions_max_resources_reached():
	$Message.text = "Max resources reached!"
	await get_tree().create_timer(2.0).timeout
	$Message.text = ""


func _on_back_button_pressed():
	current_view.hide()
	current_view = $MainView
	$"../../StarMap".hide()
	selected_planet.get_node("Camera3D").make_current()
	$StarMapView.choosing_destination = false
	if $StarMapView.selected_body:
		$StarMapView.select_body($StarMapView.selected_body)
	
	$"../../SolarSystem".show()
	$"../../OmniLight3D".show()
	$BackButton.hide()
	
	$MainView.show()
	$Resources.show()
	selected_planet.get_node("UI/BuildingsLabel").show()
	$Message.text = ""
