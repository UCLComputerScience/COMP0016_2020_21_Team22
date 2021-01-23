using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlotter : MonoBehaviour {
 
 // Name of the input file, no extension
 public string inputfile;
 
  
 // List for holding data from CSV reader
 private List<Dictionary<string, object>> pointList;
 
 // Use this for initialization
 void Start () {
 
 // Set pointlist to results of function Reader with argument inputfile
 pointList = CSVReader.Read(inputfile);
 
 //Log to console
 Debug.Log(pointList);
 }
  
}
