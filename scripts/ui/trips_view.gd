extends Control


var trips: Array[Trip]


func start_trip():
	$NoTripsLabel.hide()
	var new_trip = load("res://scenes/trip.tscn").instantiate()
	new_trip.destination = get_node("/root/Main/SolarSystem/%s" % $"/root/Main/SafeArea/UI/StarMapView".selected_destination.name)
	new_trip.get_node("Src").texture = load("res://assets/UI/%s.png" % $"/root/Main/SafeArea/UI".selected_planet.name.to_lower())
	new_trip.get_node("SrcLabel").text = $"/root/Main/SafeArea/UI".selected_planet.name
#	new_trip.get_node("ProgressBar").max_value = dist
	new_trip.get_node("Dest").texture = load("res://assets/UI/%s.png" % new_trip.destination.name.to_lower())
	new_trip.get_node("DestLabel").text = new_trip.destination.name
	trips.push_back(new_trip)
	$ScrollContainer/VBoxContainer.add_child(new_trip)


func end_trip(trip):
	trips.erase(trip)
	$ScrollContainer/VBoxContainer.remove_child(trip)
	if trips.is_empty():
		$NoTripsLabel.show()
