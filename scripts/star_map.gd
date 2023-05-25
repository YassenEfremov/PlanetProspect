extends MeshInstance3D


var selected_star: Area3D = null

#var immediate_mesh


#func _ready():
#	immediate_mesh = ImmediateMesh.new()
#	var material = ORMMaterial3D.new()
#	immediate_mesh.surface_begin(Mesh.PRIMITIVE_LINES, material)
#	immediate_mesh.surface_add_vertex($"../Camera3D".position + Vector3.DOWN * 2)
#	immediate_mesh.surface_add_vertex(Input.get_magnetometer())
#	immediate_mesh.surface_end()
#	material.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
#	material.albedo_color = Color.GREEN


func _process(delta):
#	$MainView/Sensors.text = "gyro: %s\nmag: %s" % [str(Input.get_gyroscope().round()), str(Input.get_magnetometer().round())]
	$North.position = Input.get_magnetometer().normalized() * 3


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
