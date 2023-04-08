extends Panel


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_close_button_pressed():
	hide()


func _on_news_button_pressed():
	show()
	$APODGetter.request("https://api.nasa.gov/planetary/apod?api_key=dfIiGsmYjKIEPz4Uz03Od9gR0W14gA8wzG7blbXP")


func _on_apod_getter_request_completed(result, response_code, headers, body):
	var body_json = JSON.parse_string(body.get_string_from_utf8())
	var label = Label.new()
	label.text = body_json.title
	add_child(label)

	$ImageDownloader.request(body_json.url)


func _on_image_downloader_request_completed(result, response_code, headers, body):
	var image = Image.new()
	var image_error = image.load_jpg_from_buffer(body)
	if image_error != OK:
		print("An error occurred while trying to display the image.")

	var texture = ImageTexture.new()
	texture.create_from_image(image)
	var rect = TextureRect.new()
	add_child(rect)
	rect.texture = texture
#	rect.expand_mode = TextureRect.EXPAND_IGNORE_SIZE
#	rect.stretch_mode = TextureRect.STRETCH_KEEP_ASPECT
#	rect.anchors_preset = PRESET_FULL_RECT
