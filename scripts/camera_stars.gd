extends Camera3D


var MIN_ZOOM: float
var MAX_ZOOM: float = 1000


func _physics_process(delta):
	# Orienting the star map is easier than orienting the camera
	pass


func _unhandled_input(event):
	if current:
		if event is InputEventScreenTouch:
			if event.pressed:
				Global.touches[event.index] = event
			else:
				Global.touches.erase(event.index)
				Global.drags.erase(event.index)

		if event is InputEventScreenDrag:
			Global.drags[event.index] = event
			if Global.drags.size() == 2:
				camera_zoom(Global.drags[0], Global.drags[1])	# hmmmm... breaks sometimes


func camera_zoom(drag1: InputEventScreenDrag, drag2: InputEventScreenDrag):
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
