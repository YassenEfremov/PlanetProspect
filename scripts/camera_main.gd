extends Camera3D


var MIN_ZOOM: float
var MAX_ZOOM: float = 100
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
	var angle_x = -drag.relative.x * (focused_object.position.distance_to(position) / 4000)
	var angle_y = -drag.relative.y * (focused_object.position.distance_to(position) / 4000)
 
	# Horizontal rotation
	var new_pos_x = focused_object.position \
		+ (position - focused_object.position).rotated(focused_object.global_transform.basis.y, angle_x)
	rotate(focused_object.global_transform.basis.y, angle_x)
	position = new_pos_x
	
	# Vertical rotation
	var new_pos_y = focused_object.position \
		+ (position - focused_object.position).rotated(global_transform.basis.x, angle_y)
	if rotation_degrees.x <= -75.0:
		if angle_y > 0:		# Allow only moving up
			rotate(global_transform.basis.x, angle_y)
			position = new_pos_y
	elif rotation_degrees.x >= 75.0:
		if angle_y < 0:		# Allow only moving down
			rotate(global_transform.basis.x, angle_y)
			position = new_pos_y
	else:
		rotate(global_transform.basis.x, angle_y)
		position = new_pos_y

# looks better but DOESN'T WORK????
#	var delta_x = (position - focused_object.position).rotated(focused_object.global_transform.basis.y, angle_x) \
#				- (position - focused_object.position)
#	var delta_y = (position - focused_object.position).rotated(global_transform.basis.x, angle_y) \
#				- (position - focused_object.position)
#	translate(delta_x)


func camera_zoom(drag1: InputEventScreenDrag, drag2: InputEventScreenDrag):
	MIN_ZOOM = focused_object.get_shape().radius * 1.5
	
	# Calculate zoom
	var zoom = drag1.position.distance_to(drag2.position) / (drag1.position - drag1.relative).distance_to(drag2.position - drag2.relative)
	
	# edge case
	if zoom == 0 or zoom > 10:
		return;
	
	# Limit camera zoom
	if focused_object.position.distance_to(position) >= MAX_ZOOM:
		if zoom > 1:	# Allow only zooming in
			position = focused_object.position.lerp(position, 1 / zoom)
	
	elif focused_object.position.distance_to(position) <= MIN_ZOOM:
		if zoom < 1:	# Allow only zooming out
			position = focused_object.position.lerp(position, 1 / zoom)
	
	else:
		position = focused_object.position.lerp(position, 1 / zoom)
