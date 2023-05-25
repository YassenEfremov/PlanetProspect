extends MarginContainer


func _ready():
	var viewportSize = get_viewport().size
	var safeArea = DisplayServer.get_display_safe_area()
	
	$UI/DebugText.text += "viewport: %s x %s\n" % [viewportSize.x, viewportSize.y]
	$UI/DebugText.text += "safe: %s x %s\n" % [safeArea.size.x, safeArea.size.y]
	$UI/DebugText.text += "    start_x: %s\n" % [safeArea.position.x]
	$UI/DebugText.text += "    end_x: %s\n" % [safeArea.end.x]
	
	var top: int = safeArea.position.y
	var left: int = 32	# mergin is needed here because by default it's zero!
	if safeArea.position.x > 0:
		# Decrease the margin because it's too big, most devices shouldn't have THAT big of a notch
		left = safeArea.position.x * 0.66
	
	var bottom: int = viewportSize.y - safeArea.end.y
	var right: int = viewportSize.x - safeArea.end.x
	if safeArea.position.x > 0:
		# Same offset here because otherwise everything 3D is off-center from the UI
		right = safeArea.position.x * 0.66
	
	add_theme_constant_override("margin_top", top)
	add_theme_constant_override("margin_left", left)

	add_theme_constant_override("margin_bottom", bottom)
	add_theme_constant_override("margin_right", right)
