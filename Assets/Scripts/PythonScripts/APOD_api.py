import os
import sys
import requests
from urllib.request import urlretrieve
import json
from datetime import date
from PIL import Image

def fetchAPOD(api_key, pathname):
	URL_APOD = "https://api.nasa.gov/planetary/apod"
	params = {
	  'api_key' : api_key,
#	  'date' : "2022-03-08",
	  'hd' : 'True'
	}
	
	response = requests.get(URL_APOD,params=params).json()
	img_url = requests.get(response["hdurl"])
	
	image_pathname = pathname + "\\Pictures"
	if not os.path.isdir(image_pathname):
		os.makedirs(image_pathname)

	
	img_name = response["hdurl"].split("/")[-1]
	response['picture'] = img_name
	
	img_filename = os.path.join(image_pathname, img_name)
	file = open(img_filename, "wb")
	file.write(img_url.content)
	file.close()

	
api_key = "RkKaZUhTLv3tJ2ar6t8NQxrnG9w5tZtmQjREUWsj"
filepath = os.pardir + "\\Resources\\APOD"
fetchAPOD(api_key, filepath)
