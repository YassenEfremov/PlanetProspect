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

public class ContentGatheringScript : MonoBehaviour
{
    public void Awake()
    {
        string path = Application.dataPath + "/Resources/APOD/";
        bool PictureFlag = true;
        for (int i = 0; i < 7; i++)
        {
            string filename = DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd") + ".json";
            bool jsonfileflag = File.Exists(Path.Combine(path + "JSON/", filename));
            if (!jsonfileflag)
            {
                Debug.Log("DownLoading");
                StartCoroutine(APODFetch(DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd"), success => {
                    if (!success)
                    {
                        PictureFlag = false;
                    }
                }));
                

            }
        }
        if (File.Exists((path + "JSON/" + DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + ".json")))
        {
            if(PictureFlag is true)
            {
                File.Delete(path + "JSON/" + DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + ".json");
                string[] PicToDelete = Directory.GetFiles(path + "Pictures/", DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + ".*");
                File.Delete(PicToDelete[0]);
            }
        }
    }
    // Start is called before the first frame update
    public void Start()
    {
        GameObject goContainer = new GameObject();
        goContainer.name = "goContainer";
        goContainer.tag = "clone";
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
        goImage.tag = "clone";
        goImage.AddComponent<RawImage>();
        goImage.transform.SetParent(this.transform, false);

        GameObject goTitle = new GameObject();
        goTitle.name = "goTitle";
        goTitle.tag = "clone";
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
        goDesc.tag = "clone";
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
                CreateObjects(goContainer, goTitle, goImage, goDesc, textures[i], FileList[i]);
            }
            catch (Exception e) { }
        }
        Destroy(goContainer);
        Destroy(goImage);
        Destroy(goTitle);
        Destroy(goDesc);
    }

    public void DeleteClones()
    {
        var clones = GameObject.FindGameObjectsWithTag("clone");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }

    public void CreateObjects(GameObject goContainer, GameObject goTitle, GameObject goImage, GameObject goDesc,UnityEngine.Object texture, string JsonString)
    {
        var JsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonString);
        GameObject TmpParent = Instantiate(goContainer, this.transform);
        GameObject TmpTitle = Instantiate(goTitle, TmpParent.transform);
        GameObject TmpImg = Instantiate(goImage, TmpParent.transform);
        GameObject TmpDesc = Instantiate(goDesc, TmpParent.transform);

        Text t = TmpTitle.GetComponent<Text>();
        t.text = JsonDict["title"];

        RawImage ri = TmpImg.GetComponent<RawImage>();
        Texture2D tmp_texture = (Texture2D)texture;
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


    IEnumerator APODFetch(string date, System.Action<bool> success)
    {
        var url = "https://api.nasa.gov/planetary/apod?api_key=RkKaZUhTLv3tJ2ar6t8NQxrnG9w5tZtmQjREUWsj&date=" + date;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();
        string result = webRequest.downloadHandler.text;
        Debug.Log(result);
        if(result.Contains("Date must be between Jun 16, 1995 and Mar 11, 2022."))
        {
            success(false);
            yield break;
        }
        var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
        string ImageExtension = values["hdurl"].Substring((values["hdurl"].LastIndexOf('.')));
        string ImageFilePath = Application.dataPath + "/Resources/APOD/Pictures/" + values["date"] + ImageExtension;
        string JsonFilePath = Application.dataPath + "/Resources/APOD/JSON/" + values["date"] + ".json";
        File.WriteAllText(JsonFilePath, result);
        using (WebClient webClient = new WebClient())
        {
            yield return new WaitForSeconds(3);
            webClient.DownloadFile(values["hdurl"], ImageFilePath);
            Debug.Log("Download Success");
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
