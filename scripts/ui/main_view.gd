extends Control


#func _unhandled_input(event):
#	if event is InputEventScreenTouch:
#		if not event.pressed:
#			$"../".selected_planet.click_building($"../".selected_planet.selected_building)


#func _on_star_map_button_button_down():
#	$StarMapButton/Button/AnimationPlayer.play("StarMapButton")
#
#
#func _on_star_map_button_button_up():
#	$StarMapButton/Button/AnimationPlayer.play_backwards("StarMapButton")
#
#
#func _on_trips_button_button_down():
#	$TripsButton/Button/AnimationPlayer.play("TripsButton")
##	print($TripsButton/Button.get_theme_stylebox("normal", "Button").bg_color)
##	var style: StyleBox = $TripsButton/Button.get_theme_stylebox("pressed", "Button")
##	var tween = $TripsButton/Button.create_tween()
##	tween.tween_property($TripsButton/Button, "theme/bg_color", Color.RED, 1)
#
##	tween.interpolate_value($TripsButton/Button.get_theme_stylebox("normal", "StyleBoxFlat").bg_color, Color())
##	tween.interpolate_value($TripsButton/Button.position.x, 100, 1, 2, Tween.TRANS_LINEAR, Tween.EASE_IN)
##	tween.tween_callback($TripsButton/Button.queue_free)
#
#
#func _on_trips_button_button_up():
#	$TripsButton/Button/AnimationPlayer.play_backwards("TripsButton")
