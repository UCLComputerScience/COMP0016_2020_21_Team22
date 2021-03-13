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

    public Button thresholdButton;
    public Canvas thresholdCanvas;
    public InputField X;
    public InputField Y;
    public InputField Z;

    private float xMax, xMin, yMax, yMin, zMax, zMin;
    private float xThresh, yThresh, zThresh;
    
    // Use this for initialization
    void Start()
    {
        xThresh = xMax;
        yThresh = yMax;
        zThresh = zMax;
        scaleControl.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        thresholdButton.onClick.AddListener(onThresholdClick);
        X.onEndEdit.AddListener(delegate { onEnd(); }) ;
        Y.onEndEdit.AddListener(delegate { onEnd(); });
        Z.onEndEdit.AddListener(delegate { onEnd(); });
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

        plotScale = scaleControl.value;
        DrawGraph(plotScale, number_of_data);

    }
    void onThresholdClick()
    {
        bool b = thresholdCanvas.gameObject.activeSelf;
        thresholdCanvas.gameObject.SetActive(!b);
    }
    void onEnd()
    {
        try
        {
            xThresh = float.Parse(X.text);
        }
        catch (Exception e)
        {
            //Debug.LogError(e);
            showMessage("Invalid X threshold");
        }
        try
        {
            yThresh = float.Parse(Y.text);
        }
        catch (Exception e)
        {
            //Debug.LogError(e);
            showMessage("Invalid Y threshold");
        }
        try
        {
            zThresh = float.Parse(Z.text);
        }
        catch (Exception e)
        {
            //Debug.LogError(e);
            showMessage("Invalid Z threshold");
        }
        ThresholdUpdate();
    }
    private void ThresholdUpdate()
    {
        //draw the thresholdsn (limit to decimal numbers for inputfield on unity editor)
        cleanPrevious("not ben");
        xMax = getlarger(xMax, xThresh);
        xMin = getsmaller(xMin, xThresh);
        yMax = getlarger(yMax, yThresh);
        yMin = getsmaller(yMin, yThresh);
        zMax = getlarger(zMax, zThresh);
        zMin = getsmaller(zMin, zThresh);
        float xThresOnPlot = NoDivideZero(xMax, xMin, xThresh);
        float yThresOnPlot = NoDivideZero(yMax, yMin, yThresh);
        float zThresOnPlot = NoDivideZero(zMax, zMin, zThresh);
        DrawLine(new Vector3(xThresOnPlot * plotScale, -1, 0), new Vector3(xThresOnPlot, 1, 0) * plotScale, new Color(xThresOnPlot, -1, 0), new Color(xThresOnPlot, 1, 0), "not ben");
        DrawLine(new Vector3(-1, yThresOnPlot * plotScale, 0), new Vector3(1, yThresOnPlot, 0) * plotScale, new Color(-1, yThresOnPlot, 0), new Color(1, yThresOnPlot, 0), "not ben");
        DrawLine(new Vector3(0, 0, zThresOnPlot * plotScale), new Vector3(0, 1, zThresOnPlot) * plotScale, new Color(0, 0, zThresOnPlot), new Color(0, 1, zThresOnPlot), "not ben");
    }
    private void showMessage(string message)
    {
        GameObject messageObject = new GameObject();
        messageObject.transform.SetParent(thresholdCanvas.transform);
        Text messageText = messageObject.AddComponent<Text>();
        messageText.fontSize = 20;
        messageText.text = message;
        messageText.transform.position = new Vector3(360, 1200, 0);
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

    private void Update()
    {
        frameCounter += 1;
        if (frameCounter > 200)
        {
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
        ThresholdUpdate();
    }

    private void DrawGraph(float scale, int dataNum)
    {
        //Debug.Log("drawgraph used");
        // Set pointlist to results of function Reader with argument inputfile
        //Debug.Log(transform.position);
        pointList = CSVReader.Read(inputfile);
        if(pointList == null)
        {
            return;
        }
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
        xMax = FindMaxValue(xName);
        yMax = FindMaxValue(yName);
        zMax = FindMaxValue(zName);

        // Get minimums of each axis
        xMin = FindMinValue(xName);
        yMin = FindMinValue(yName);
        zMin = FindMinValue(zName);

        centerPoint = new Vector3(((xMax + xMin) / 2) / (xMax - xMin), ((yMax + yMin) / 2) / (yMax - yMin), ((zMax + zMin) / 2) / (zMax - zMin)) * scale;

        cleanPrevious("ben");

        //draw the axes
        DrawLine(new Vector3(-1, 0, 0), new Vector3(1, 0, 0) * scale, Color.red, Color.red, "ben");
        DrawLine(new Vector3(0, -1, 0), new Vector3(0, 1, 0) * scale, Color.blue, Color.blue, "ben");
        DrawLine(new Vector3(0, 0, -1), new Vector3(0, 0, 1) * scale, Color.green, Color.green, "ben");
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
                DrawLine(new Vector3(x, y, z) * scale, new Vector3(x1, y1, z1) * scale, new Color(0, y, 0), new Color(0, y1, 0), "ben");
            }
        }
        displayValue(indexCounter);
    }

    private float getsmaller(float a, float b)
    {
        if (a > b)
        {
            return b;
        }
        return a;
    }
    private float getlarger(float a, float b)
    {
        if (a > b)
        {
            return a;
        }
        return b;
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

    private void DrawLine(Vector3 start, Vector3 end, Color startcolor, Color endColor, string name)
    {
        GameObject myLine = new GameObject();
        myLine.name = name;
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.startColor = startcolor;
        lr.endColor = endColor;
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

    private float NoDivideZero(float max, float min, float number)
    {
        float value = 0;
        if (max != min)
        {
            value =
                (number - min) / (max - min);
        }
        return value;
    }
    private void cleanPrevious(string name)
    {
        var objects = FindObjectsOfType<GameObject>();
        foreach (GameObject line in objects)
        {
            if (line.name == name)
            {
                GameObject.Destroy(line);
                //Debug.Log(line);
            }
        }
    }
    private void displayValue(int n)
    {
        if (pointList == null)
        {
            return;
        }
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
            float xPlot = NoDivideZero(xMax, xMin, n, xName);
            float yPlot = NoDivideZero(yMax, yMin, n, yName);
            float zPlot = NoDivideZero(zMax, zMin, n, zName);
            ballPoint = new Vector3(xPlot, yPlot, zPlot) * plotScale;
            //Debug.Log("printing value");
            cleanPrevious("alice");

            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    ballPoint,
                    Quaternion.identity);
            dataPoint.GetComponent<Renderer>().material.color =
                new Color(xPlot, yPlot, zPlot);
            dataPoint.name = "alice";

            GameObject dataPointX = Instantiate(
                    PointPrefab,
                    new Vector3(ballPoint.x,0,0),
                    Quaternion.identity);
            dataPointX.GetComponent<Renderer>().material.color =
                new Color(xPlot, 0, 0);
            dataPointX.name = "alice";

            GameObject dataPointY = Instantiate(
                    PointPrefab,
                    new Vector3(0, ballPoint.y, 0),
                    Quaternion.identity);
            dataPointY.GetComponent<Renderer>().material.color =
                new Color(0, yPlot, 0);
            dataPointY.name = "alice";

            GameObject dataPointZ = Instantiate(
                    PointPrefab,
                    new Vector3(0, 0, ballPoint.z),
                    Quaternion.identity);
            dataPointZ.GetComponent<Renderer>().material.color =
                new Color(0, 0, zPlot);
            dataPointZ.name = "alice";
        }
    }
}

