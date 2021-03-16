using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        stateMessage.text = selectedName + " is selected";
        cleanseBefore("data message");
        int filelength = pointList.Count;
        //Debug.Log("file lenght is: " + filelength + " data num is: " + dataNum + "////////////////////////");
        if (filelength - 1 > dataNum)
        {
            pointList = pointList.GetRange(filelength - dataNum - 1, dataNum);
            GameObject.Find("data content").GetComponent<GridLayoutGroup>().constraintCount = dataNum;
        }
        else
        {
            GameObject.Find("data content").GetComponent<GridLayoutGroup>().constraintCount = filelength;
        }

        List<string> columnList = new List<string>(pointList[1].Keys);

        int numberOfColomns = 0;
        foreach (string colomnName in columnList)
        {
            int index;

            addTextToScreen(colomnName, "data message");
            for (index = 0; index < pointList.Count - 1; index++)
            {
                try
                {
                    string content = System.Convert.ToString(pointList[index][colomnName]);
                    addTextToScreen(content, "data message");
                }
                catch (KeyNotFoundException)
                {
                    addTextToScreen("N/A", "data message");
                }
            }
            numberOfColomns += 1;

        }

    }
    private void addTextToScreen(string content, string name)
    {
        GameObject textHolder = Instantiate(dataTextToInstantiate);
        textHolder.transform.SetParent(GameObject.Find("data content").transform);
        textHolder.transform.localScale = new Vector2(1.0f, 1.0f);
        textHolder.name = name;
        Text message = textHolder.GetComponent<Text>();
        message.fontSize = 20;
        message.text = content;
    }
}
