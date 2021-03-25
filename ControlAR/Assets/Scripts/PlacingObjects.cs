using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacingObjects : MonoBehaviour
{
    public GameObject gameObjectToInstantiate;
    public GameObject backgroundForListToInstantiate;
    public GameObject dataTextToInstantiate;
    public GameObject realTimeDataBackgroundToInstantiate;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    private bool startPlacing = false;


    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public Button stopPlacingButton;
    public Button selectMachine;
    public GameObject canvasHolder;
    public GameObject buttonHolder;

    public TextMeshProUGUI stateMessage;
    public string selectedName = null;
    public Button stopEditButton;

    Vector3 touchAngle = new Vector3(0, 0, 0);

    bool editMachine = false;

    private float counter = 0;
    private float update = 0;
    private bool isMachineSelected = false;

    public Button showDataButton;
    public Button showFunctionButton;
    public GameObject functionList;
    public GameObject dataList;

    public GameObject dataHolder;
    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }
    // Update is called once per frame

    void Start()
    {
        stopEditButton.onClick.AddListener(onStopEditing);
        selectMachine.onClick.AddListener(onSelectClick);
        stopPlacingButton.onClick.AddListener(TaskOnClick);
        showDataButton.onClick.AddListener(onShowDataClicked);
        showFunctionButton.onClick.AddListener(onShowFunctionClick);
        stopEditButton.gameObject.SetActive(false);
        stopPlacingButton.gameObject.SetActive(false);
    }
    public void onShowFunctionClick()
    {
        //show the list of actions you can do
        functionList.SetActive(!functionList.activeSelf);
    }
    public void onShowDataClicked()
    {
        //show real time data
        dataList.SetActive(!dataList.activeSelf);
    }

    private void onStopEditing()
    {
        //when "confirm" is clicked to stop the object from moving
        startPlacing = false;
        //stopEditButton.gameObject.SetActive(false);
        stopPlacingButton.gameObject.SetActive(!stopPlacingButton.IsActive());
        if (stopPlacingButton.IsActive())
        {
            stopEditButton.GetComponentInChildren<Text>().text = "confirm";
            editMachine = true;
        }
        else
        {
            stopEditButton.GetComponentInChildren<Text>().text = "move";
            editMachine = false;
        }
        stateMessage.text = "press and hold on a machine to configure";
        cleanseBefore("data message");
    }

    public void cleanseBefore(string name)
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
    void onSelectClick()
    {
        //whe the user clicks on the button to selected a machine to place on the AR envrionment
        // spawn a canvas with all the machines the account have.
        var allMachines = System.IO.Directory.GetDirectories(Application.persistentDataPath);
        GameObject selectionCanvas = Instantiate(canvasHolder);
        selectionCanvas.name = "machine selection page";
        int counter = 0;
        foreach (string machine in allMachines)
        {
            string machineName = getDifferenceAtEnd(machine, Application.persistentDataPath + "/{0}");
            int column = counter % 2 + 1;
            int row = counter / 2 + 1;
            GameObject machineButtonsHolder = Instantiate(buttonHolder, new Vector3(column * 300 - 90, 1440 - row * 200, 0), Quaternion.identity);
            machineButtonsHolder.transform.SetParent(selectionCanvas.transform);

            Button machineButtons = machineButtonsHolder.GetComponent<Button>();
            if (machineButtons != null)
            {
                //for all machines, create a button with a listener
                machineButtons.transform.name = machine;
                machineButtons.onClick.AddListener(delegate { onMachineSelected(machineName); });
            }
            Text MachineName = machineButtons.GetComponentInChildren<Text>();
            if (MachineName != null)
            {
                MachineName.text = machineName;
            }

            counter += 1;
        }


    }
    public string getDifferenceAtEnd(string a, string b)
    {
        // get the difference at the end  for two strings
        //this is only used to get the machine name and its path
        //the path is the root plus machine name
        //so only need the length of the root path
        string c;
        c = a.Substring(b.Length);
        return c;
    }
    public void onMachineSelected(string machineName)
    {
        try
        {
            GameObject.Destroy(GameObject.Find("machine selection page"));
        }
        catch (Exception)
        {

        }
        if (machineName != null && GameObject.Find(machineName) == null)
        {
            selectedName = machineName;
            whenMachinedSelectedByTouch();
            stateMessage.text = machineName + " is selected \ntouch the plane to place machine";
            stopPlacingButton.gameObject.SetActive(true);
            editMachine = true;
            startPlacing = true;
            GameObject download = new GameObject();
            download.name = machineName + " downloader";
            string link = System.IO.File.ReadAllText(Application.persistentDataPath + "/{0}" + machineName + "/link.txt");
            download.AddComponent<CSVDownloader>().fileName = link;
            download.GetComponent<CSVDownloader>().path = machineName;
        }
    }

    public void TaskOnClick()
    {
        //when changing between moving the machine and rotation/size
        startPlacing = !startPlacing;

        Text buttonText = stopPlacingButton.GetComponentInChildren<Text>();
        if(buttonText != null)
        {
            if (startPlacing)
            {
                buttonText.text = "size and\nrotation";
            }
            else
            {
                buttonText.text = "change position";
            }
        }
    }

    void Update()
    {
        // get the finger input and do corresponding actions
        if(Input.touchCount >= 2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) && editMachine == true)
        {
            //control size
            Vector2 touch0, touch1;
            float distance;
            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;
            distance = Vector2.Distance(touch0, touch1)/100;
            GameObject.Find(selectedName).transform.localScale = new Vector3(distance, distance, distance);
        }
        else if (Input.touchCount == 1)
        {
            //if finger is not touching any UI e.g. buttons, ...
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hitinfo;
                if (isMachineSelected == true && editMachine == false)
                {
                    //deselect
                    if (Physics.Raycast(ray, out hitinfo))
                    {
                        if (GameObject.Find(hitinfo.transform.name).tag != "machine")
                        {
                            deselected();
                        }
                    }
                }
                if (editMachine == true && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    if (startPlacing == true)
                    {
                        //move model
                        placingObject(selectedName);
                    }
                    else if (Input.GetTouch(0).phase != TouchPhase.Began)
                    {
                        //rotation
                        Vector2 touchRotatePosition = Input.GetTouch(0).position;
                        Vector3 newTouchAngle = new Vector3(touchRotatePosition.x, touchRotatePosition.y, 0);
                        var theMachine = GameObject.Find(selectedName);
                        theMachine.transform.Rotate(Camera.main.transform.right, (newTouchAngle.y - touchAngle.y) / 2, Space.World);
                        theMachine.transform.Rotate(Camera.main.transform.up, (touchAngle.x - newTouchAngle.x) / 2, Space.World);
                        touchAngle = newTouchAngle;
                    }
                    else
                    {
                        touchAngle = Input.GetTouch(0).position;
                    }
                }
                else if (isMachineSelected != true)
                {
                    if (Physics.Raycast(ray, out hitinfo))
                    {
                        if (GameObject.Find(hitinfo.transform.name).tag == "machine")
                        {
                            //press and hold to select machine
                            if (counter < 0.5f)
                            {
                                counter += Time.deltaTime;
                            }
                            else
                            {
                                selectedName = hitinfo.transform.name;
                                counter = 0;
                                Handheld.Vibrate();
                                whenMachinedSelectedByTouch();
                            }
                        }
                    }
                }
            }
        }
        update += Time.deltaTime;
        if (update > 5.0f && selectedName != null && dataList.activeSelf)  
        {
            // refresh the real time data
            displayData();
            update = 0;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            onMachineSelected(null);
        }
        if(Input.touchCount == 0)
        {
            counter = 0;
        }
    }

    private void whenMachinedSelectedByTouch()
    {
        stateMessage.text = selectedName + " is selected";
        showDataButton.gameObject.SetActive(true);
        showFunctionButton.gameObject.SetActive(true);
        isMachineSelected = true;
        editMachine = false;
        startPlacing = false;
        stopPlacingButton.gameObject.SetActive(false);
        stopEditButton.gameObject.SetActive(true);

    }
    public void deselected()
    {
        stateMessage.text = "press and hold on a machine to configure";
        startPlacing = false;
        editMachine = false;
        isMachineSelected = false;
        stopPlacingButton.gameObject.SetActive(false);
        stopEditButton.gameObject.SetActive(false);
        //functionList.SetActive(false);
        //dataList.SetActive(false);
        showDataButton.gameObject.SetActive(false);
        showFunctionButton.gameObject.SetActive(false);
    }
    public void displayData()
    {
        // display data in the scroll view 
        List<Dictionary<string, object>>  pointList = CSVReader.Read(selectedName); 
        if(pointList == null)
        {
            stateMessage.text = selectedName + " is selected\nLoading....";
            return;
        }
        stateMessage.text = selectedName + " is selected";
        cleanseBefore("data message");
        List<string> columnList = new List<string>(pointList[1].Keys); 
        int length = pointList.Count - 1;
        foreach (string x in columnList)
        {
            string colomnName = x;
            
            addTextToScreen(x + ": ","data message",1);
            try
            {
                string content = System.Convert.ToString(pointList[length][colomnName]);
                addTextToScreen(content, "data message", 0); 
            }
            catch (KeyNotFoundException)
            {
                addTextToScreen("N/A", "data message", 0);
            }

        }


    }
    public void addTextToScreen(string content, string name, int n)
    {
        GameObject textHolder = Instantiate(dataTextToInstantiate);
        textHolder.transform.SetParent(dataHolder.transform);
        textHolder.transform.localScale = new Vector2(1.0f, 1.0f);
        textHolder.name = name;
        Text message = textHolder.GetComponent<Text>();
        if (n == 0)
        {
            message.alignment = TextAnchor.MiddleRight;
        }
        else if (n == 1)
        {
            message.alignment = TextAnchor.MiddleLeft;
        }
        message.fontSize = 20;
        message.text = content;
    }
    private void placingObject(string name)
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }
        if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if (GameObject.Find(name) == null)
            {
                GameObject spwanedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                spwanedObject.transform.name = name;
                spwanedObject.tag = "machine";
                spwanedObject.transform.SetParent(transform);
            }
            else
            { 
                if(GameObject.Find(name).tag == "machine")
                {
                    GameObject.Find(name).transform.position = hitPose.position;
                }
            }
        }
    }
}
