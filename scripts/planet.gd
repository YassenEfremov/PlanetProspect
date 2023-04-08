extends Area3D


@export var rotation_speed: float
@export var max_buildings: int
@export var giant: bool

var axis
var last_press_pos
var building_to_place: PackedScene = null
var buildings = []
var selected_building: Area3D = null

signal building_placed(steel_cost: int)

func _ready():
	axis = Vector3(0, cos($Axis.rotation.x), sin($Axis.rotation.x))
	if not giant:
#		await $UI/BuildingsLabel/Current.tree_entered
#		await $UI/BuildingsLabel/Max.tree_entered
		while not $UI/BuildingsLabel/Current or not $UI/BuildingsLabel/Max:
			pass # Wait for child labels to load
		$UI/BuildingsLabel/Current.text = str(buildings.size())
		$UI/BuildingsLabel/Max.text = str(max_buildings)


func _process(delta):
	$MeshInstance3D.rotate(axis, delta * rotation_speed)


func select_building(building):
	if !selected_building:
		# Newly selected building
#		var actions = building.get_node("Actions")
#		if actions.get_parent():
#			actions.get_parent().remove_child(actions)
#		get_node("/root/Main/UI/MainView").add_child(actions)
		
		selected_building = building
		building.get_node("MeshInstance3D/Outline").visible = true
		for button in selected_building.action_buttons:
			button.show()
#		actions.show()
#		get_node("/root/Main/UI/MainView/Actions").show()
	
	else:
		if building != selected_building:
			# Change selected building
#			get_node("/root/Main/UI/MainView").remove_child($UI.selected_planet.get_node("Actions"))
			
			for button in selected_building.action_buttons:
				button.hide()
			selected_building.get_node("MeshInstance3D/Outline").visible = false
			selected_building = building
			building.get_node("MeshInstance3D/Outline").visible = true
			for button in building.action_buttons:
				button.show()
		
		else:
			# Deselect current building
#			var actions = get_node("/root/Main/UI/MainView/Actions")
#			if actions.get_parent():
#				actions.get_parent().remove_child(actions)
#			building.add_child(actions)
			
			for button in selected_building.action_buttons:
				button.hide()
			building.get_node("MeshInstance3D/Outline").visible = false
			selected_building = null
#			actions.hide()


func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventScreenTouch:
		if event.pressed:
			last_press_pos = event.position
			
		if not event.pressed and building_to_place and event.position == last_press_pos:
#			$UI.select_planet(self)

			var building = building_to_place.instantiate()
			$MeshInstance3D.add_child(building) # This comes before setting the position and rotation!
#			building.scale = Vector3(0.5, 0.5, 0.5)
			building.look_at_from_position(position, normal)
			building.rotate_object_local(Vector3.RIGHT, PI/2)

			buildings.push_back(building)
			building_placed.emit(building.steel_cost)
			building_to_place = null
			$UI/BuildingsLabel/Current.text = str(buildings.size())
			$"../../UI/Message".text = ""
			$"../../UI/Message".hide()
			$"../../UI/MainView/CancelButton".hide()


func _on_build_view_building_selected(building_name: String):
	building_to_place = load("res://scenes/buildings/%s.tscn" % building_name)


func _on_cancel_button_pressed():
	building_to_place = null


func remove_building():
	$MeshInstance3D.remove_child(selected_building)
	buildings.erase(selected_building)
	$UI/BuildingsLabel/Current.text = str(buildings.size())
	select_building(selected_building)
