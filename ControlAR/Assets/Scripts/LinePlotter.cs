using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class LinePlotter : MonoBehaviour
{
    // Name of the input file, no extension
    public string inputfile;

    public Slider scaleControl;
    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    // Indices for columns to be assigned
    public int columnX = 0;
    public int columnY = 1;
    public int columnZ = 2;

    // Full column names
    public string xName;
    public string yName;
    public string zName;

    public Color lineColor;

    public float plotScale = 10;


    // Use this for initialization
    void Start()
    {
        scaleControl.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        DrawGraph(plotScale);
    }

    public void ValueChangeCheck()
    {
        plotScale = scaleControl.value;
        //Debug.Log(scaleControl.value);
        DrawGraph(plotScale);
    }

    private void DrawGraph(float scale)
    {
        // Set pointlist to results of function Reader with argument inputfile
        pointList = CSVReader.Read(inputfile);

        //Log to console
        //Debug.Log(pointList);

        // Declare list of strings, fill with keys (column names)
        List<string> columnList = new List<string>(pointList[1].Keys);

        // Print number of keys (using .count)
        Debug.Log("There are " + columnList.Count + " columns in the CSV");

        foreach (string key in columnList)
            Debug.Log("Column name is " + key);

        // Assign column name from columnList to Name variables
        xName = columnList[columnX];
        yName = columnList[columnY];
        zName = columnList[columnZ];

        // Get maxes of each axis
        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);

        // Get minimums of each axis
        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);


        var objects = FindObjectsOfType<GameObject>();
        foreach (GameObject line in objects)
        {
            if (line.name == "ben")
            {
                GameObject.Destroy(line);
                Debug.Log(line);
            }
        }
        //Loop through Pointlist
        for (var i = 0; i < pointList.Count; i++)
        {
            // Get value in poinList at ith "row", in "column" Name, normalize
            float x =
                (System.Convert.ToSingle(pointList[i][xName]) - xMin)
                / (xMax - xMin);

            float y =
                (System.Convert.ToSingle(pointList[i][yName]) - yMin)
                / (yMax - yMin);

            float z =
                (System.Convert.ToSingle(pointList[i][zName]) - zMin)
                / (zMax - zMin);

            if (i < pointList.Count - 1)
            {
                float x1 =
                    (System.Convert.ToSingle(pointList[i + 1][xName]) - xMin)
                    / (xMax - xMin);

                float y1 =
                    (System.Convert.ToSingle(pointList[i + 1][yName]) - yMin)
                    / (yMax - yMin);

                float z1 =
                    (System.Convert.ToSingle(pointList[i + 1][zName]) - zMin)
                    / (zMax - zMin);
                DrawLine(new Vector3(x, y, z) * scale, new Vector3(x1, y1, z1) * scale, lineColor);
            }
        }
        DrawLine(new Vector3(-1, 0, 0), new Vector3(1, 0, 0) * scale, lineColor);
        DrawLine(new Vector3(0,-1, 0), new Vector3(0, 1, 0) * scale, lineColor);
        DrawLine(new Vector3(0, 0, -1), new Vector3(0, 0, 1) * scale, lineColor);
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
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        //GameObject.Destroy(myLine, duration);
    }
}