extends MeshInstance3D


var selected_star: Area3D = null


func select_star(star):
	if !selected_star:
		# Newly selected building
#		var actions = star.get_node("Actions")
#		if actions.get_parent():
#			actions.get_parent().remove_child(actions)
#		get_node("/root/Main/UI/StarMapView").add_child(actions)
		
		selected_star = star
		star.get_node("MeshInstance3D/Outline").visible = true
#		actions.show()
#		get_node("/root/Main/UI/StarMapView/Actions").show()
	
	else:
		if star != selected_star:
			# Change selected building
#			get_node("/root/Main/UI/StarMapView").remove_child(selected_star.get_node("Actions"))

			selected_star.get_node("MeshInstance3D/Outline").visible = false
			selected_star = star
			star.get_node("MeshInstance3D/Outline").visible = true
		
		else:
			# Deselect current building
#			var actions = get_node("/root/Main/UI/StarMapView/Actions")
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
