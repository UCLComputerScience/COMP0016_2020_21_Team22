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
    string selectedName = null;
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
    private void onShowFunctionClick()
    {
        functionList.SetActive(!functionList.activeSelf);
    }
    private void onShowDataClicked()
    {
        dataList.SetActive(!dataList.activeSelf);
    }

    private void onStopEditing()
    {
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
    void onSelectClick()
    {
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
    private string getDifferenceAtEnd(string a, string b)
    {
        string c;
        c = a.Substring(b.Length);
        return c;
    }
    void onMachineSelected(string machineName)
    {
        GameObject.Destroy(GameObject.Find("machine selection page"));
        if (machineName != null && GameObject.Find(machineName) == null)
        {
            selectedName = machineName;
            whenMachinedSelectedByTouch();
            stateMessage.text = machineName + "is selected \ntouch the plane to place machine";
            stopPlacingButton.gameObject.SetActive(true);
            editMachine = true;
            startPlacing = true;
            GameObject download = new GameObject();
            string link = System.IO.File.ReadAllText(Application.persistentDataPath + "/{0}" + machineName + "/link.txt");
            download.AddComponent<Program>().fileName = link;
            download.GetComponent<Program>().path = machineName;
        }
    }

    void TaskOnClick()
    {
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
        if(Input.touchCount >= 2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) && editMachine == true)
        {
            Vector2 touch0, touch1;
            float distance;
            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;
            distance = Vector2.Distance(touch0, touch1)/100;
            GameObject.Find(selectedName).transform.localScale = new Vector3(distance, distance, distance);
        }
        else if (Input.touchCount == 1)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hitinfo;
                if (isMachineSelected == true && editMachine == false)
                {
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
                        placingObject(selectedName);
                    }
                    else if (Input.GetTouch(0).phase != TouchPhase.Began)
                    {
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
    private void deselected()
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
    private void displayData()
    {
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
    private void addTextToScreen(string content, string name, int n)
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
