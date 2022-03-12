using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class testXml : MonoBehaviour
{
    void Start()
    {

    }
    // Start is called before the first frame update
    public void ButtonClick()
    {
        GameObject Content = GameObject.Find("Content");
        ContentGatheringScript scr;
        scr = Content.GetComponent<ContentGatheringScript>();
        scr.DeleteClones();

        string URLString = "https://www.nasa.gov/rss/dyn/educationnews.rss";
        XmlTextReader reader = new XmlTextReader(URLString);
        bool flag = true;
        string title = "";
        string link = "";
        string description = "";
        int reset = 0;

        GameObject goContainer = new GameObject();
        goContainer.name = "goContainer";
        goContainer.tag = "clone";
        goContainer.AddComponent<VerticalLayoutGroup>();
        goContainer.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        goContainer.GetComponent<VerticalLayoutGroup>().spacing = 10;
        goContainer.GetComponent<VerticalLayoutGroup>().childControlWidth = false;
        goContainer.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
        goContainer.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = true;
        goContainer.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
        goContainer.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(20, 20, 20, 20);
        goContainer.AddComponent<ContentSizeFitter>();
        goContainer.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        GameObject goLink = new GameObject();
        goLink.name = "goLink";
        goLink.tag = "clone";
        goLink.AddComponent<Text>();
        goLink.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1000);
        goLink.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        goLink.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        goLink.GetComponent<Text>().alignByGeometry = true;
        goLink.GetComponent<Text>().resizeTextForBestFit = true;
        goLink.GetComponent<Text>().resizeTextMaxSize = 60; 
        goLink.transform.SetParent(Content.transform, false);

        GameObject goTitle = new GameObject();
        goTitle.name = "goTitle";
        goTitle.tag = "clone";
        goTitle.AddComponent<Text>();
        goTitle.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1000);
        goTitle.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        goTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        goTitle.GetComponent<Text>().alignByGeometry = true;
        goTitle.GetComponent<Text>().resizeTextForBestFit = true;
        goTitle.GetComponent<Text>().resizeTextMaxSize = 100;


        goTitle.transform.SetParent(Content.transform, false);

        GameObject goDesc = new GameObject();
        goDesc.name = "goDesc";
        goDesc.tag = "clone";
        goDesc.AddComponent<Text>();
        goDesc.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1000);
        goDesc.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80);
        goDesc.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        goDesc.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        goDesc.GetComponent<Text>().alignByGeometry = true;
        goDesc.GetComponent<Text>().resizeTextForBestFit = true;
        goDesc.GetComponent<Text>().resizeTextMaxSize = 30;

        goDesc.transform.SetParent(Content.transform, false);



        while (reader.Read())
        {
            if (reset == 3)
            {
                CreateObjects(Content, goContainer, goTitle, goLink, goDesc, title, link, description);
                reset = 0;
            }
            else
            {
                if (reader.NodeType is XmlNodeType.Element)
                {
                    if (reader.Name == "title")
                    {
                        if (flag)
                        {
                            continue;
                        }
                        reader.Read();
                        title = reader.Value;
                        reset++;
                    }
                    else if (reader.Name == "link")
                    {
                        if (flag)
                        {
                            flag = false;
                            continue;
                        }
                        reader.Read();
                        link = reader.Value;
                        reset++;
                    }
                    else if (reader.Name == "description")
                    {
                        if (flag)
                        {
                            continue;
                        }
                        reader.Read();
                        description = reader.Value;
                        reset++;
                    }
                }
            }
        }
        Destroy(goContainer);
        Destroy(goLink);
        Destroy(goTitle);
        Destroy(goDesc);

    }
    public void CreateObjects(GameObject Content, GameObject goContainer, GameObject goTitle, GameObject goLink, GameObject goDesc, string title, string link, string description)
    {
        GameObject TmpParent = Instantiate(goContainer, Content.transform);
        GameObject TmpTitle = Instantiate(goTitle, TmpParent.transform);
        GameObject TmpDesc = Instantiate(goDesc, TmpParent.transform);
        GameObject TmpLink = Instantiate(goLink, TmpParent.transform);

        Text t = TmpTitle.GetComponent<Text>();
        t.text = title;

        Text l = TmpLink.GetComponent<Text>();
        l.text = link;
        Text d = TmpDesc.GetComponent<Text>();
        d.text = description;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
