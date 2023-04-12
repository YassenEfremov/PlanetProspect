extends Control


var selected_planet: Planet = null
var current_view

signal camera_rotate(delta: InputEventScreenDrag)
signal camera_zoom(delta1: InputEventScreenDrag, delta2: InputEventScreenDrag)


func _ready():
	current_view = $MainView
	select_planet($"../SolarSystem/Earth")


func _process(delta):
#	$MainView/Sensors.text = "gyro: %s\nmag: %s" % [str(Input.get_gyroscope().round()), str(Input.get_magnetometer().round())]
	pass


func _unhandled_input(event):
	if event is InputEventScreenTouch:
		if event.pressed:
			Global.touches[event.index] = event
		else:
			Global.touches.erase(event.index)

	if event is InputEventScreenDrag:
		Global.touches[event.index] = event
		if Global.touches.size() == 2:
			camera_zoom.emit(Global.touches[0], Global.touches[1])
		else:
			camera_rotate.emit(event)


func select_planet(planet):
	if !selected_planet:
		# Newly selected planet
#		var actions = planet.get_node("Actions")
#		if actions.get_parent():
#			actions.get_parent().remove_child(actions)
#		get_node("/root/Main/UI").add_child(actions)
		
		selected_planet = planet
		planet.get_node("MeshInstance3D/Outline").visible = true
		planet.get_node("Axis").visible = true
#		actions.show()
#		get_node("/root/Main/UI/MainView/Actions").show()
	
	else:
		if planet != selected_planet:
			# Change selected planet
#			get_node("/root/Main/UI").remove_child(selected_planet.get_node("Actions"))

			selected_planet.get_node("MeshInstance3D/Outline").visible = false
			selected_planet.get_node("Axis").visible = false
			selected_planet = planet
			planet.get_node("MeshInstance3D/Outline").visible = true
			planet.get_node("Axis").visible = true
		
		else:
			# Deselect current planet
#			var actions = get_node("/root/Main/UI/Actions")
#			if actions.get_parent():
#				actions.get_parent().remove_child(actions)
#			planet.add_child(actions)
			
			selected_planet = null
			planet.get_node("MeshInstance3D/Outline").visible = false
			planet.get_node("Axis").visible = false
#			actions.hide()
#			get_node("/root/Main/UI/MainView/Actions").hide()


func _on_star_map_button_pressed():
	current_view = $StarMapView
	$"../Camera3D".save_camera = $"../Camera3D".duplicate()
	var star_map = $"../StarMap"
	star_map.position = $"../Camera3D".position
	
	$"../SolarSystem".hide()
	$"../OmniLight3D".hide()
	$MainView.hide()
	$Resources.hide()
	selected_planet.get_node("UI/BuildingsLabel").hide()
	
	$"../Camera3D".star_view = true
	star_map.show()
	$StarMapView.show()
	$BackButton.show()


func _on_build_button_pressed():
	current_view = $BuildView
	$"../Camera3D".save_camera = $"../Camera3D"
	
	$MainView.hide()
	
	$BuildView.show()
	$BackButton.show()


func _on_build_view_building_selected(building_name):
	_on_back_button_pressed()
	$Message.text = "Click somewhere on the planet to place the building"
	$Message.show()
	$MainView/CancelButton.show()


func _on_cancel_button_pressed():
	$Message.text = ""
	$Message.hide()
	$MainView/CancelButton.hide()


func _on_build_view_max_buildings_reached():
	_on_back_button_pressed()
	$Message.text = "Maximum number of buildings reached!"
	$Message.show()
	await get_tree().create_timer(2.0).timeout
	$Message.hide()


func _on_build_view_not_enough_resources():
	$Message.text = "Not enough resources!"
	$Message.show()
	await get_tree().create_timer(2.0).timeout
	$Message.text = ""
	$Message.hide()


func _on_actions_max_resources_reached():
	$Message.text = "Max resources reached!"
	$Message.show()
	await get_tree().create_timer(2.0).timeout
	$Message.text = ""
	$Message.hide()


func _on_back_button_pressed():
	current_view.hide()
	current_view = $MainView
	$"../StarMap".hide()
	$"../Camera3D".star_view = false
	$"../Camera3D".transform = $"../Camera3D".save_camera.transform
	$"../Camera3D".fov = $"../Camera3D".save_camera.fov
#	$"../StarMap".select_star($"../StarMap".selected_star)
	
	$"../SolarSystem".show()
	$"../OmniLight3D".show()
	$BackButton.hide()
	
	$MainView.show()
	$Resources.show()
	selected_planet.get_node("UI/BuildingsLabel").show()
