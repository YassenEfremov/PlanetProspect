extends Control


var selected_body = null
var choosing_destination: bool = false
var selected_destination = null

#var planets_shown = true
#var moons_shown = true
#var stars_shown = true


func select_body(body):
	if !selected_body:
		# Newly selected body
		selected_body = body
		if choosing_destination: get_node("%sStats/DestButton" % body.name).show()
		get_node("%sStats" % body.name).show()
	
	else:
		if body != selected_body:
			# Change selected body
#			get_node("/root/Main/SafeArea/UI/MainView").remove_child($UI.selected_planet.get_node("Actions"))
			
			if choosing_destination: get_node("%sStats/DestButton" % body.name).hide()
			get_node("%sStats" % selected_body.name).hide()
			selected_body = body
			if choosing_destination: get_node("%sStats/DestButton" % body.name).show()
			get_node("%sStats" % body.name).show()
		
		else:
			# Deselect current body
			if choosing_destination: get_node("%sStats/DestButton" % body.name).hide()
			get_node("%sStats" % body.name).hide()
			selected_body = null


func _on_planet_button_toggled(button_pressed: bool):
#	planets_shown = !planets_shown
	$Actions/PlanetButton/Icon.modulate = Color(0.3, 0.3, 0.3) if button_pressed else Color.WHITE


func _on_moon_button_toggled(button_pressed: bool):
#	moons_shown = !moons_shown
	$Actions/MoonButton/Icon.modulate = Color(0.3, 0.3, 0.3) if button_pressed else Color.WHITE


func _on_star_button_toggled(button_pressed: bool):
#	stars_shown = !stars_shown
	$Actions/StarButton/Icon.modulate = Color(0.3, 0.3, 0.3) if button_pressed else Color.WHITE


func _on_dest_button_pressed():
	choosing_destination = false
	selected_destination = selected_body
	$"../"._on_back_button_pressed()
	$"../Message".text = ""
	if $"/root/Main/SafeArea/UI/MainView/Actions/AddRocketButton".disabled:
		$"/root/Main/SafeArea/UI/MainView/Actions/LaunchButton".disabled = false
	$"../MainView/Actions/DestinationButton/Icon".texture = load("res://assets/UI/%s.png" % selected_destination.name.to_lower())
