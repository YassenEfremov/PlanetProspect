extends Area3D


@export var locked: bool

var last_press_pos: Vector2


func _process(delta):
#	get_node("/root/Main/SafeArea/UI/StarMapView/%sStats" % name).visible = not get_viewport().get_camera_3d().is_position_behind(global_transform.origin)
	var stats_panel = get_node("/root/Main/SafeArea/UI/StarMapView/%sStats" % name)
	if stats_panel.visible:
		stats_panel.position = get_viewport().get_camera_3d().unproject_position(global_transform.origin)
		stats_panel.position += Vector2(20, -stats_panel.size.y / 2)


func _input_event(camera, event, position, normal, shape_idx):
	if event is InputEventScreenTouch:
		if event.pressed:
			last_press_pos = event.position
			
		if not event.pressed and event.position == last_press_pos:
#			get_node("/root/Main/SafeArea/UI/StarMapView/%sStats" % name).position = camera.unproject_position(position)
			$"../../../SafeArea/UI/StarMapView".select_body(self)


func toggle_visibility():
	visible = !visible
