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
	
	for tile in $RotatingStuff/Grid.get_children():
		if tile.get_child_count() > 0:
			var building: Building = tile.get_children()[0]
			buildings[tile] = building
			$UI/BuildingsLabel/Current.text = str(buildings.size())
	
	$UI/BuildingsLabel/Current.text = str(buildings.size())
	$UI/BuildingsLabel/Max.text = str(max_buildings)


func _process(delta):
	$RotatingStuff.rotate(axis, (1 / day_length) * delta)


func select_building(building: Building):
	selected_building = building
	building.get_node("%Outline").visible = true
	for button in selected_building.action_buttons:
		button.show()
	
	var tile = buildings.find_key(selected_building)
	
	var arrays = []
	arrays.resize(Mesh.ARRAY_MAX)
	arrays[Mesh.ARRAY_VERTEX] = tile.get_shape().get_faces()
	
	var highlight_mat = StandardMaterial3D.new()
	highlight_mat.transparency = BaseMaterial3D.TRANSPARENCY_ALPHA
	highlight_mat.albedo_color = Color(1.0, 1.0, 1.0, 0.5)

	var highlight_mesh = ArrayMesh.new()
	highlight_mesh.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES, arrays)
	highlight_mesh.surface_set_material(highlight_mesh.get_surface_count() - 1, highlight_mat)
	
	var tile_highlight = MeshInstance3D.new()
	tile_highlight.mesh = highlight_mesh
	tile_highlight.name = "TileHighlight"
	tile.add_child(tile_highlight)
	tile_highlight.position += Vector3.UP * 0.01	# prevents texture flickering


func deselect_building(building: Building):
	for button in selected_building.action_buttons:
		button.hide()
	
	var tile = buildings.find_key(selected_building)
	tile.remove_child(tile.get_node("TileHighlight"))
	
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
			var tile = $RotatingStuff/Grid.shape_owner_get_owner($RotatingStuff/Grid.shape_find_owner(shape_idx))
			if buildings.has(tile):
				return
			
			var building = building_to_place.instantiate()
			tile.add_child(building) # This comes before setting the position and rotation!
			building.scale = Vector3(0.5, 0.5, 0.5)

			buildings[tile] = building
			building_placed.emit(building)
			building_to_place = null
			$UI/BuildingsLabel/Current.text = str(buildings.size())
			$"../../SafeArea/UI/Message".text = ""
			$"../../SafeArea/UI/MainView/CancelButton".hide()


func _on_cancel_button_pressed():
	building_to_place = null


func remove_selected_building():
	var building_copy = selected_building
	deselect_building(selected_building)
	
	var tile = buildings.find_key(building_copy)
	tile.remove_child(building_copy)
	buildings.erase(tile)
	for button in building_copy.action_buttons:
		button.disabled = false
	$UI/BuildingsLabel/Current.text = str(buildings.size())
