@tool
extends EditorScript


func _run():
	for i in range(162):
#		print(get_scene().get_node("Node3D/Area3D/Tile%d" % i))
		print(i)
#		get_scene().get_node("Area3D/Tile%d" % i).reparent(get_scene().get_node("Area3D/CollisionShape3D%d" % i))
#		get_scene().get_node("Area3D/Tile%d" % i).owner = get_scene()
	pass
