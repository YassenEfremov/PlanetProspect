class_name Planet extends Node3D


@export var radius: float
@export var day_length: float
@export var max_buildings: int

var axis
var last_press_pos: Vector2
var building_to_place: PackedScene = null
var buildings: Dictionary = {}
var selected_building: Building = null

signal building_placed(building: Building)


func _ready():
	axis = Vector3(sin($Axis.rotation.x), cos($Axis.rotation.x), 0)
#		await $UI/BuildingsLabel/Current.tree_entered
#		await $UI/BuildingsLabel/Max.tree_entered
	while not $UI/BuildingsLabel/Current or not $UI/BuildingsLabel/Max:
		pass # Wait for child labels to load
	$UI/BuildingsLabel/Current.text = str(buildings.size())
	$UI/BuildingsLabel/Max.text = str(max_buildings)


func _process(delta):
	$Area3D.rotate(axis, (1 / day_length) * delta)


func select_building(building: Building):
	selected_building = building
	building.get_node("%Outline").visible = true
	
	var tile = buildings.find_key(selected_building)
	var selected_tile = ArrayMesh.new()
	var selected_mat = StandardMaterial3D.new()
	selected_mat.transparency = BaseMaterial3D.TRANSPARENCY_ALPHA
	selected_mat.albedo_color = Color(1.0, 1.0, 1.0, 0.5)
	var arrays = []
	arrays.resize(Mesh.ARRAY_MAX)
	arrays[Mesh.ARRAY_VERTEX] = tile.get_shape().get_faces()

	selected_tile.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES, arrays)
	selected_tile.surface_set_material(selected_tile.get_surface_count() - 1, selected_mat)
	var m = MeshInstance3D.new()
	m.mesh = selected_tile
	m.name = "SelectedHighlight"
	tile.add_child(m)
	# The position of the selected building is basically the normal vector of the tile
	m.position = selected_building.position.normalized() * 0.01
	
	for button in selected_building.action_buttons:
		button.show()


func deselect_building(building: Building):
	for button in selected_building.action_buttons:
		button.hide()
	
	var tile = buildings.find_key(selected_building)
	tile.remove_child(tile.get_node("SelectedHighlight"))
	
	building.get_node("%Outline").visible = false
	selected_building = null


func click_building(building: Building):
	if !selected_building:
		# Newly selected building
		select_building(building)
	
	else:
		if building != selected_building:
			# Change selected building
			deselect_building(selected_building)
			select_building(building)
		
		else:
			# Deselect current building
			deselect_building(building)


func _on_area_3d_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventScreenTouch:
		if event.pressed:
			last_press_pos = event.position
			
		if not event.pressed and building_to_place and event.position == last_press_pos:
			var tile = $Area3D.shape_owner_get_owner($Area3D.shape_find_owner(shape_idx))
			var tile_points = tile.get_shape().get_faces()
#			if buildings[tile]:
#				pass
			
			var building = building_to_place.instantiate()
			tile.add_child(building) # This comes before setting the position and rotation!
			building.scale = Vector3(0.5, 0.5, 0.5)
			# This way of aligning with the normal works, no idea how, don't touch it for now
			building.look_at_from_position(position, position - normal)
			building.rotate_object_local(basis.x, PI/2)
			# Each hexagon tile is made up of 4 triangles, the one we care about is the center one
			# Vertecies 9, 10 and 11 are the center triangle's vertecies
			# Therefore their average is the center of the tile
			building.position = (tile_points[9] + tile_points[10] + tile_points[11]) / 3

			buildings[tile] = building
			building_placed.emit(building)
			building_to_place = null
			$UI/BuildingsLabel/Current.text = str(buildings.size())
			$"../../SafeArea/UI/Message".text = ""
			$"../../SafeArea/UI/MainView/CancelButton".hide()


func _on_cancel_button_pressed():
	building_to_place = null


func remove_building():
	buildings.find_key(selected_building).remove_child(selected_building)
	buildings.erase(selected_building)
	for button in selected_building.action_buttons:
		button.disabled = false
	$UI/BuildingsLabel/Current.text = str(buildings.size())
	click_building(selected_building)
