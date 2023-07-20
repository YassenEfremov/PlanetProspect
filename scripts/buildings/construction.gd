extends Node3D


@export var build_time: int

signal finished


func _ready():
	$BuildTimer.wait_time = build_time
	$ProgressBar/SubViewportContainer/SubViewport/ProgressBar.max_value = build_time
	$ProgressBar/SubViewportContainer/SubViewport/ProgressBar.value = $ProgressBar/SubViewportContainer/SubViewport/ProgressBar.max_value


func _process(delta):
	var time_left = $BuildTimer.time_left
	var time_left_str = ""
	if time_left >= 3600:
		time_left_str += "%d h" % int(time_left / 3600)
		time_left -= int(time_left / 3600) * 3600
	if time_left >= 60:
		time_left_str += " %d m" % int(time_left / 60)
		time_left -= int(time_left / 60) * 60
	if time_left > 0:
		time_left_str += " %d s" % int(time_left)
	
	$ProgressBar/Value.text = time_left_str
	$ProgressBar/SubViewportContainer/SubViewport/ProgressBar.value = $BuildTimer.time_left


func _on_build_timer_timeout():
	hide()
	if not get_parent().permanent and $"/root/Main/SafeArea/UI".selected_planet.selected_building == get_parent():
		$"/root/Main/SafeArea/UI".selected_planet.deselect_building($"../")
	finished.emit()

func start():
	$BuildTimer.start()
