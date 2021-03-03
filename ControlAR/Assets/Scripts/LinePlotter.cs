using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEditor;

public class LinePlotter : MonoBehaviour
{
    // Name of the input file, no extension
    public string inputfile;
    public int number_of_data;
    public Slider scaleControl;
    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;


    // Indices for columns to be assigned
    public int columnX = 0;
    public int columnY = 1;
    public int columnZ = 2;
    public int newcolumn = 4;

    // Full column names
    public string xName;
    public string yName;
    public string zName;
    public string newName;

    public Color lineColor;
    public Material lineMat;

    public float plotScale = 1;
    private int frameCounter = 0;

    public Vector3 centerPoint = new Vector3(0, 0, 0);
    public Vector3 ballPoint = new Vector3(0, 0, 0);
    public Text dataType1;
    public Text dataType2;
    public Text dataType3;
    public Text dataTime;

    public Button lastData;
    public Button nextData;
    private int indexCounter = 0;

    public GameObject PointPrefab;

    public Dropdown xAxis;
    public Dropdown yAxis;
    public Dropdown zAxis;

    public GameObject lineHolder;
    // Use this for initialization
    void Start()
    {
        scaleControl.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lastData.onClick.AddListener(onLastClick);
        nextData.onClick.AddListener(onNexttClick);
        DrawGraph(plotScale, number_of_data);
        displayValue(indexCounter);
        List<string> columnList = new List<string>(pointList[1].Keys);
        foreach(var item in columnList)
        {
            xAxis.options.Add(new Dropdown.OptionData() { text = item });
            yAxis.options.Add(new Dropdown.OptionData() { text = item });
            zAxis.options.Add(new Dropdown.OptionData() { text = item });
        }
        xAxis.value = columnX;
        yAxis.value = columnY;
        zAxis.value = columnZ;
        xAxis.onValueChanged.AddListener(delegate { OnXChanged(); });
        yAxis.onValueChanged.AddListener(delegate { OnYChanged(); });
        zAxis.onValueChanged.AddListener(delegate { OnZChanged(); });
    }

    void OnXChanged()
    {
        columnX = xAxis.value;
        DrawGraph(plotScale, number_of_data);
        Debug.Log(columnX);
    }
    void OnYChanged()
    {
        columnY = yAxis.value;
        DrawGraph(plotScale, number_of_data);
        Debug.Log(columnY);
    }
    void OnZChanged()
    {
        columnZ = zAxis.value;
        DrawGraph(plotScale, number_of_data);
        Debug.Log(columnZ);
    }
    void onLastClick()
    {
        if (indexCounter > 0)
        {
            indexCounter = indexCounter - 1;
        }
        displayValue(indexCounter);
    }

    void onNexttClick()
    {
        if(indexCounter < number_of_data)
        {
            indexCounter = indexCounter + 1;
        }
        displayValue(indexCounter);
    }
    //IEnumerator ExampleCoroutine()
    //{
    //    plotScale = scaleControl.value;
    //    for (int i = 0; i < 50; i++)
    //    {
    //        //AssetDatabase.Refresh();
    //        //Resources.Load("1184899729_8e5db2aefea84a28a1437ec1088bbd0b_1.csv");
    //        DrawGraph(plotScale, number_of_data);
    //        yield return new WaitForSeconds(3);
    //    }
    //}
    private void Update()
    {
        frameCounter = frameCounter + 1;
        if (frameCounter % 120 == 0)
        {
            //Debug.Log("/////////////frame update///////////////////");
            plotScale = scaleControl.value;
            DrawGraph(plotScale, number_of_data);
            frameCounter = 0;
        }
    }

    public void ValueChangeCheck()
    {
        plotScale = scaleControl.value;
        //Debug.Log(scaleControl.value);
        DrawGraph(plotScale, number_of_data);
    }

    private void DrawGraph(float scale, int dataNum)
    {
        //Debug.Log("drawgraph used");
        // Set pointlist to results of function Reader with argument inputfile
        //Debug.Log(transform.position);
        pointList = CSVReader.Read(inputfile);
        int filelength = pointList.Count;
        //Debug.Log("file lenght is: " + filelength + " data num is: " + dataNum + "////////////////////////");
        if (filelength - 1 > dataNum)
        {
            pointList = pointList.GetRange(filelength - dataNum - 1, dataNum);
        }


        //Log to console
        //Debug.Log(pointList);

        // Declare list of strings, fill with keys (column names)
        List<string> columnList = new List<string>(pointList[1].Keys);


        // Print number of keys (using .count)
        //Debug.Log("There are " + columnList.Count + " columns in the CSV");

        //foreach (string key in columnList)
        //    Debug.Log("Column name is " + key);

        // Assign column name from columnList to Name variables

        xName = columnList[columnX];
        yName = columnList[columnY];
        zName = columnList[columnZ];
        newName = columnList[newcolumn];

        //Debug.Log("x: " + xName + " y: " + yName + " z: " + zName);
        // Get maxes of each axis
        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);

        // Get minimums of each axis
        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);

        centerPoint = new Vector3(((xMax + xMin) / 2) / (xMax - xMin), ((yMax + yMin) / 2) / (yMax - yMin), ((zMax + zMin) / 2) / (zMax - zMin)) * scale;

        var objects = FindObjectsOfType<GameObject>();
        foreach (GameObject line in objects)
        {
            if (line.name == "ben")
            {
                GameObject.Destroy(line);
                //Debug.Log(line);
            }
        }

        //Loop through Pointlist
        for (var i = 0; i < pointList.Count; i++)
        {
            // Get value in poinList at ith "row", in "column" Name, normalize

            float x = NoDivideZero(xMax, xMin, i, xName);
            float y = NoDivideZero(yMax, yMin, i, yName);
            float z = NoDivideZero(zMax, zMin, i, zName);

            //float x = System.Convert.ToSingle(pointList[i][xName]);
            //float y = System.Convert.ToSingle(pointList[i][yName]);
            //float z = System.Convert.ToSingle(pointList[i][zName]);

            if (i < pointList.Count - 1)
            {
                float x1 = NoDivideZero(xMax, xMin, i + 1, xName);
                float y1 = NoDivideZero(yMax, yMin, i + 1, yName);
                float z1 = NoDivideZero(zMax, zMin, i + 1, zName);

                //float x1 = System.Convert.ToSingle(pointList[i+1][xName]);
                //float y1 = System.Convert.ToSingle(pointList[i+1][yName]);
                //float z1 = System.Convert.ToSingle(pointList[i+1][zName]);
                DrawLine(new Vector3(x, y, z) * scale, new Vector3(x1, y1, z1) * scale, lineColor);
            }
        }
        DrawLine(new Vector3(-1, 0, 0), new Vector3(1, 0, 0) * scale, lineColor);
        DrawLine(new Vector3(0, -1, 0), new Vector3(0, 1, 0) * scale, lineColor);
        DrawLine(new Vector3(0, 0, -1), new Vector3(0, 0, 1) * scale, lineColor);
        displayValue(indexCounter);
    }
    private float FindMaxValue(string columnName)
    {
        //set initial value to first value
        float maxValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing maxValue if new value is larger
        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                maxValue = Convert.ToSingle(pointList[i][columnName]);
        }

        //Spit out the max value
        return maxValue;
    }

    private float FindMinValue(string columnName)
    {

        float minValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing minValue if new value is smaller
        for (var i = 0; i < pointList.Count; i++)
        {
            if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(pointList[i][columnName]);
        }

        return minValue;
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.name = "ben";
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material.color = color;
        //lr.startColor = color;
        //lr.endColor = color;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    private float NoDivideZero(float max, float min, int counter, string column)
    {
        float value = 0;
        if (max != min)
        {
            value =
                (System.Convert.ToSingle(pointList[counter][column]) - min)
                / (max - min);
        }
        return value;
    }

    private void displayValue(int n)
    {
        //Debug.Log("n is: " + n);
        if (pointList.Count > n && n > 0)
        {
            float x = System.Convert.ToSingle(pointList[n][xName]);
            float y = System.Convert.ToSingle(pointList[n][yName]);
            float z = System.Convert.ToSingle(pointList[n][zName]);
            string time = System.Convert.ToString(pointList[n][newName]);
            dataType1.text = ("X: " + xName + ": " + x);
            dataType2.text = ("Y: " + yName + ": " + y);
            dataType3.text = ("Z: " + zName + ": " + z);
            dataTime.text = (newName + ": " + time);
            float xPlot = NoDivideZero(FindMaxValue(xName), FindMinValue(xName), n, xName);
            float yPlot = NoDivideZero(FindMaxValue(yName), FindMinValue(yName), n, yName);
            float zPlot = NoDivideZero(FindMaxValue(zName), FindMinValue(zName), n, zName);
            ballPoint = new Vector3(xPlot, yPlot, zPlot) * plotScale;
            //Debug.Log("printing value");
            var objects = FindObjectsOfType<GameObject>();
            foreach (GameObject ball in objects)
            {
                if (ball.name == "alice")
                {
                    GameObject.Destroy(ball);
                    //Debug.Log(line);
                }
            }

            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    ballPoint,
                    Quaternion.identity);
            dataPoint.name = "alice";
        }
    }
}

