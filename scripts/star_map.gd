extends Node3D


var selected_star: Area3D = null
var drifted = false


func _physics_process(delta):
	if drifted and (abs(Input.get_gyroscope().x) < 1 or abs(Input.get_gyroscope().y) < 1 or abs(Input.get_gyroscope().z) < 1):
		# wait to calm down and orient everything
		await get_tree().create_timer(0.5).timeout
		orient()
		drifted = false
	
	if abs(Input.get_gyroscope().x) > 10 or abs(Input.get_gyroscope().y) > 10 or abs(Input.get_gyroscope().z) > 10:
		# looking around too fast
		drifted = true
	
	$RotatingStuff.rotate($Camera3D.basis.x.normalized(), -Input.get_gyroscope().x / 60)
	$RotatingStuff.rotate($Camera3D.basis.y.normalized(), -Input.get_gyroscope().y / 60)
	$RotatingStuff.rotate($Camera3D.basis.z.normalized(), -Input.get_gyroscope().z / 60)


func orient():
	var g = Input.get_accelerometer().normalized()
	var N = Plane(g).project(Input.get_magnetometer()).normalized()
	$RotatingStuff.look_at(N, -g)


func select_star(star):
	if !selected_star:
		# Newly selected building
#		var actions = star.get_node("Actions")
#		if actions.get_parent():
#			actions.get_parent().remove_child(actions)
#		get_node("/root/Main/SafeArea/UI/StarMapView").add_child(actions)
		
		selected_star = star
		star.get_node("MeshInstance3D/Outline").visible = true
#		actions.show()
#		get_node("/root/Main/SafeArea/UI/StarMapView/Actions").show()
	
	else:
		if star != selected_star:
			# Change selected building
#			get_node("/root/Main/SafeArea/UI/StarMapView").remove_child(selected_star.get_node("Actions"))

			selected_star.get_node("MeshInstance3D/Outline").visible = false
			selected_star = star
			star.get_node("MeshInstance3D/Outline").visible = true
		
		else:
			# Deselect current building
#			var actions = get_node("/root/Main/SafeArea/UI/StarMapView/Actions")
#			if actions.get_parent():
#				actions.get_parent().remove_child(actions)
#			star.add_child(actions)
			
			selected_star = null
			star.get_node("MeshInstance3D/Outline").visible = false
#			actions.hide()


func _on_planet_button_toggled(button_pressed: bool):
	get_tree().call_group("planets", "toggle_visibility")


func _on_moon_button_toggled(button_pressed: bool):
	get_tree().call_group("moons", "toggle_visibility")


func _on_star_button_toggled(button_pressed: bool):
	get_tree().call_group("stars", "toggle_visibility")
