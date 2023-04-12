extends Control


var selected_body = null


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
