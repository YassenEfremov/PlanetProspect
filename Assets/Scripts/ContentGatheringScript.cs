using System;
using System.IO;
using System.Collections;
using System.Web;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine.Networking;
//using UnityEngine.UnityWebRequestModule;


public class JsonInfo
{
    public string date;
    public string title;
}
public class ContentGatheringScript : MonoBehaviour
{
    void Awake()
    {
        string path = Application.dataPath + "/Resources/APOD/JSON";
        bool PictureFlag = true;
        for (int i = 0; i < 7; i++)
        {
            string filename = DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd") + ".json";
            bool fileflag = File.Exists(Path.Combine(path, filename));
            if (!fileflag)
            {

                try
                {

                    StartCoroutine(APODFetch(DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd")));
                }
                catch (Exception e)
                {
                    Debug.Log(DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd"));
                    PictureFlag = false;
                }

            }
        }
        if (File.Exists((path + "/" + DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + ".json")) && PictureFlag is true)
        {
            File.Delete(path + "/" + DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + ".json");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void GatherContent()
    {
        GameObject goContainer = new GameObject();
        goContainer.name = "goContainer";
        goContainer.AddComponent<VerticalLayoutGroup>();
        goContainer.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        goContainer.GetComponent<VerticalLayoutGroup>().spacing = 100;
        goContainer.GetComponent<VerticalLayoutGroup>().childControlWidth = false;
        goContainer.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
        goContainer.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = true;
        goContainer.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
        goContainer.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(50, 50, 50, 50);
        goContainer.AddComponent<ContentSizeFitter>();
        goContainer.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        GameObject goImage = new GameObject();
        goImage.name = "goImage";
        goImage.AddComponent<RawImage>();
        goImage.transform.SetParent(this.transform, false);

        GameObject goTitle = new GameObject();
        goTitle.name = "goTitle";
        goTitle.AddComponent<Text>();
        goTitle.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1000);
        goTitle.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        goTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        goTitle.GetComponent<Text>().alignByGeometry = true;
        goTitle.GetComponent<Text>().resizeTextForBestFit = true;
        goTitle.GetComponent<Text>().resizeTextMaxSize = 80;
        goTitle.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;


        goTitle.transform.SetParent(this.transform, false);

        GameObject goDesc = new GameObject();
        goDesc.name = "goDesc";
        goDesc.AddComponent<Text>();
        goDesc.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1000);
        goDesc.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 700);
        goDesc.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        goDesc.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        goDesc.GetComponent<Text>().alignByGeometry = true;
        goDesc.GetComponent<Text>().resizeTextForBestFit = true;
        goDesc.GetComponent<Text>().resizeTextMaxSize = 60;

        goDesc.transform.SetParent(this.transform, false);

        UnityEngine.Object[] jsonobject = Resources.LoadAll("APOD/JSON", typeof(TextAsset));
        List<string> FileList = new List<string>();
        foreach (var file in jsonobject)
        {
            if (!file.name.Contains(".meta"))
            {
                FileList.Add(file.ToString());
            }

        }

        UnityEngine.Object[] textures = Resources.LoadAll("APOD/Pictures", typeof(Texture2D));
        for (int i = 6; i >= 0; i--)
        {
            try
            {
                string JsonString = FileList[i];
                var JsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonString);
                GameObject TmpParent = Instantiate(goContainer, this.transform);
                GameObject TmpTitle = Instantiate(goTitle, TmpParent.transform);
                GameObject TmpImg = Instantiate(goImage, TmpParent.transform);
                GameObject TmpDesc = Instantiate(goDesc, TmpParent.transform);

                Text t = TmpTitle.GetComponent<Text>();
                t.text = JsonDict["title"];

                RawImage ri = TmpImg.GetComponent<RawImage>();
                Texture2D tmp_texture = (Texture2D)textures[i];
                ri.texture = tmp_texture;
                ri.SetNativeSize();
                var width = ri.rectTransform.rect.width;
                var height = ri.rectTransform.rect.height;
                while (width > 1024)
                {
                    width *= (float)0.8;
                    height *= (float)0.8;
                }
                ri.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                ri.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

                Text d = TmpDesc.GetComponent<Text>();
                d.text = JsonDict["explanation"];
            }
            catch (Exception e) { }
        }
        Destroy(goContainer);
        Destroy(goImage);
        Destroy(goTitle);
        Destroy(goDesc);
    }

    IEnumerator APODFetch(string date)
    {
        var url = "https://api.nasa.gov/planetary/apod?api_key=RkKaZUhTLv3tJ2ar6t8NQxrnG9w5tZtmQjREUWsj&date=" + date;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();
        string result = webRequest.downloadHandler.text;
        Debug.Log(result);
        var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
        string ImageExtension = values["hdurl"].Substring((values["hdurl"].LastIndexOf('.')));
        string ImageFilePath = Application.dataPath + "/Resources/APOD/Pictures/" + values["date"] + ImageExtension;
        string JsonFilePath = Application.dataPath + "/Resources/APOD/JSON/" + values["date"] + ".json";
        File.WriteAllText(JsonFilePath, result);
        using (WebClient webClient = new WebClient())
        {
            //yield return new WaitForSeconds(3);
            Debug.Log(values["hdurl"]);
            webClient.DownloadFile(values["hdurl"], ImageFilePath);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
