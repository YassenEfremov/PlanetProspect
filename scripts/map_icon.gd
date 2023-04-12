extends Area3D


var last_press_pos: Vector2


func _process(delta):
#	get_node("/root/Main/UI/StarMapView/%sStats" % name).visible = not get_viewport().get_camera_3d().is_position_behind(global_transform.origin)
	if get_node("/root/Main/UI/StarMapView/%sStats" % name).visible:
		get_node("/root/Main/UI/StarMapView/%sStats" % name).position = get_viewport().get_camera_3d().unproject_position(global_transform.origin)


func _input_event(camera, event, position, normal, shape_idx):
	if event is InputEventScreenTouch:
		if event.pressed:
			last_press_pos = event.position
			
		if not event.pressed and event.position == last_press_pos:
#			get_node("/root/Main/UI/StarMapView/%sStats" % name).position = camera.unproject_position(position)
			$"../../UI/StarMapView".select_body(self)
