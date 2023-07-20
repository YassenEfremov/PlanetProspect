class_name Building extends StaticBody3D


@export var steel_cost: int
@export var permanent: bool

var action_buttons: Array[BaseButton]

#func _ready():
#	if removable:	doesn't work??
#		action_buttons.push_back($"/root/Main/SafeArea/UI/MainView/Actions/RemoveBuildingButton")
