extends Camera3D


var MIN_ZOOM: float
var MAX_ZOOM: float = 1000
var star_view: bool = false
var focused_object
#var save_transform: Transform3D
var save_camera


func _ready():
#	while not focused_object:
	await $"../UI".ready
	focused_object = $"../UI".selected_planet


func _process(delta):
	if star_view:
		rotate(basis.x, Input.get_gyroscope().x / 60)
		rotate(basis.y, Input.get_gyroscope().y / 60)
		rotate(basis.z, Input.get_gyroscope().z / 60)


func _on_ui_camera_rotate(drag: InputEventScreenDrag):
	if not star_view:
		var angle = -drag.relative.x / 100
		
		var rotated_basis = transform.basis.rotated(Vector3.UP, angle)	# Rotate basis
		var rotated_origin = focused_object.position + (position - focused_object.position).rotated(Vector3.UP, angle)	# Rotate origin
		transform = Transform3D(rotated_basis, rotated_origin)

#		var from = project_ray_origin()
#		var to = from + camera.project_ray_normal(event.position) * position.distance_to(camera.position)

		# use global coordinates, not local to node
#		var query = PhysicsRayQueryParameters3D.create(position, position + Vector3(0, 0, 100))
#		var collision = get_world_3d().direct_space_state.intersect_ray(query)
#		print("Hit: ", collision)
#		if collision:
#			print("Hit at point: ", collision)


func _on_ui_camera_zoom(drag1: InputEventScreenDrag, drag2: InputEventScreenDrag):
	if not star_view:
		MAX_ZOOM = 1000
		if MIN_ZOOM != focused_object.get_node("MeshInstance3D").get_mesh().radius * 2:
			MIN_ZOOM = focused_object.get_node("MeshInstance3D").get_mesh().radius * 2
		
		# Calculate zoom
		var zoom = drag1.position.distance_to(drag2.position) / (drag1.position - drag1.relative).distance_to(drag2.position - drag2.relative)
		
		# edge case
		if zoom == 0 or zoom > 10:
			return;
		
		# Limit camera zoom
		if focused_object.position.distance_to(position) >= MAX_ZOOM:
			if zoom > 1:    # Allow only zooming in
				position = focused_object.position.lerp(position, 1 / zoom)
		
		elif focused_object.position.distance_to(position) <= MIN_ZOOM:
			if zoom < 1:    # Allow only zooming out
				position = focused_object.position.lerp(position, 1 / zoom)
		
		else:
			position = focused_object.position.lerp(position, 1 / zoom)

	else:
		MAX_ZOOM = 90
		MIN_ZOOM = 10
		
		# Calculate zoom
		var zoom = drag1.position.distance_to(drag2.position) / (drag1.position - drag1.relative).distance_to(drag2.position - drag2.relative)
		
		# edge case
		if zoom == 0 or zoom > 10:
			return;
		
		# Limit camera zoom
		if fov >= MAX_ZOOM:
			if zoom > 1:    # Allow only zooming in
				fov -= zoom
		
		elif fov <= MIN_ZOOM:
			if zoom < 1:    # Allow only zooming out
				fov += zoom
		
		else:
			fov = lerpf(MIN_ZOOM, fov, 1 / zoom)
