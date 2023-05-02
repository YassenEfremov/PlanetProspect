extends ColorRect


var trips: Array[Trip]


func start_trip():
	$NoTripsLabel.hide()
#	var new_trip = Trip.new()
	var new_trip = load("res://scenes/trip.tscn").instantiate()
	new_trip.get_node("Src").texture = load("res://assets/UI/%s.png" % $"/root/Main/UI".selected_planet.name.to_lower())
	new_trip.get_node("SrcLabel").text = $"/root/Main/UI".selected_planet.name
#	new_trip.get_node("ProgressBar").max_value = dist
	new_trip.get_node("Dest").texture = load("res://assets/UI/%s.png" % $"/root/Main/UI/StarMapView".selected_destination.name.to_lower())
	new_trip.get_node("DestLabel").text = $"/root/Main/UI/StarMapView".selected_destination.name
	trips.push_back(new_trip)
	$ScrollContainer/VBoxContainer.add_child(new_trip)


func end_trip(trip):
	trips.erase(trip)
	if trips.is_empty():
		$NoTripsLabel.show()
