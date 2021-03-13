using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class readTextLog : MonoBehaviour
{
    public string selectedName;
    public Text stateMessage;
    public GameObject dataTextToInstantiate;
    public Button refresh;
    int dataNum = 200;
    // Start is called before the first frame update
    void Start()
    {
        selectedName = "test";
        refresh.onClick.AddListener(onRefreshClick);
    }
    void onRefreshClick()
    {
        display();
    }
    private void cleanseBefore(string name)
    {

        var toDelete = FindObjectsOfType<GameObject>();
        foreach (GameObject message in toDelete)
        {
            if (message.name == name)
            {
                GameObject.Destroy(message);
            }
        }
    }
    private void display()
    {
        List<Dictionary<string, object>> pointList = CSVReader.Read(selectedName);
        if (pointList == null)
        {
            stateMessage.text = selectedName + " is selected\nLoading....";
            return;
        }
        cleanseBefore("data massage");
        int filelength = pointList.Count;
        //Debug.Log("file lenght is: " + filelength + " data num is: " + dataNum + "////////////////////////");
        if (filelength - 1 > dataNum)
        {
            pointList = pointList.GetRange(filelength - dataNum - 1, dataNum);
        }

        List<string> columnList = new List<string>(pointList[1].Keys);
        int numberOfColomns = 0;
        foreach (string colomnName in columnList)
        {
            int index;

            for (index = 0; index < pointList.Count - 1; index++)
            {
                try
                {
                    string content = System.Convert.ToString(pointList[index][colomnName]);
                    addTextToScreen(content, new Vector2((numberOfColomns - columnList.Count/2)*200 + 10, (index - pointList.Count / 2) * 30 + 10), "data message");
                }
                catch (KeyNotFoundException)
                {
                    addTextToScreen("N/A", new Vector2((numberOfColomns - columnList.Count / 2) *200 + 10, (index - pointList.Count / 2) * 30 + 10), "data message");
                }
            }
            addTextToScreen(colomnName, new Vector2((numberOfColomns - columnList.Count / 2) * 200 + 10, (index - pointList.Count/2) * 30 + 10), "data message");
            numberOfColomns += 1;

        }
        float width = (columnList.Count) * 200 + 50;
        float height = pointList.Count * 30 + 50;
        GameObject.Find("data content").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

    }
    private void addTextToScreen(string content, Vector2 position, string name)
    {
        GameObject textHolder = Instantiate(dataTextToInstantiate, position, Quaternion.identity);
        textHolder.transform.SetParent(GameObject.Find("data content").transform);
        textHolder.transform.localScale = new Vector2(1.0f, 1.0f);
        textHolder.name = name;
        Text message = textHolder.GetComponent<Text>();
        message.fontSize = 20;
        message.text = content;
    }
}
