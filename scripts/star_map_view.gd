extends Control


var selected_body = null

var planets_shown = true
var moons_shown = true
var stars_shown = true


func select_body(body):
	if !selected_body:
		# Newly selected body
		selected_body = body
		get_node("%sStats" % body.name).show()
	
	else:
		if body != selected_body:
			# Change selected body
#			get_node("/root/Main/UI/MainView").remove_child($UI.selected_planet.get_node("Actions"))
			
			get_node("%sStats" % selected_body.name).hide()
			selected_body = body
			get_node("%sStats" % body.name).show()
		
		else:
			# Deselect current body
			get_node("%sStats" % body.name).hide()
			selected_body = null


func _on_planet_button_toggled(button_pressed: bool):
	planets_shown = !planets_shown
	$Actions/PlanetButton.modulate = Color.WHITE if button_pressed else Color.DIM_GRAY


func _on_moon_button_toggled(button_pressed: bool):
	moons_shown = !moons_shown
	$Actions/MoonButton.modulate = Color.WHITE if button_pressed else Color.DIM_GRAY


func _on_star_button_toggled(button_pressed: bool):
	stars_shown = !stars_shown
	$Actions/StarButton.modulate = Color.WHITE if button_pressed else Color.DIM_GRAY
