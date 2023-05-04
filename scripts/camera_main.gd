extends Camera3D


var MIN_ZOOM: float
var MAX_ZOOM: float = 1000
var focused_object
var last_press_pos: Vector2


func _ready():
	focused_object = $"../CollisionShape3D"


func _unhandled_input(event):
	if current:
		if event is InputEventScreenTouch:
			if event.pressed:
				Global.touches[event.index] = event
				last_press_pos = event.position
			else:
				Global.touches.erase(event.index)
	#			await $"../SolarSystem/Earth".click_building()

			if not event.pressed and event.position == last_press_pos:
				var from = project_ray_origin(event.position)
				var to = from + project_ray_normal(event.position) * 50
				
				var query = PhysicsRayQueryParameters3D.create(from, to)
				var collision = get_world_3d().direct_space_state.intersect_ray(query)
				
				if collision:
					$"../".click_building(collision.collider)
				elif $"../".selected_building:
					$"../".deselect_building($"../".selected_building)

		if event is InputEventScreenDrag:
			Global.touches[event.index] = event
			if Global.touches.size() == 2:
				camera_zoom(Global.touches[0], Global.touches[1])
			else:
				camera_rotate(event)


func camera_rotate(drag: InputEventScreenDrag):
	var angle = -drag.relative.x / 100
	
	var rotated_basis = transform.basis.rotated(Vector3.UP, angle)	# Rotate basis
	var rotated_origin = focused_object.position + (position - focused_object.position).rotated(Vector3.UP, angle)	# Rotate origin
	transform = Transform3D(rotated_basis, rotated_origin)


func camera_zoom(drag1: InputEventScreenDrag, drag2: InputEventScreenDrag):
	MIN_ZOOM = focused_object.get_shape().radius * 2
	
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
