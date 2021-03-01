using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


[RequireComponent(typeof(ARRaycastManager))]
public class PlacingObjects : MonoBehaviour
{
    public GameObject gameObjectToInstantiate;
    // static List<GameObject> spwanedObject = new List<GameObject>();
    public GameObject spwanedObject;
    [SerializeField] private Toggle placeObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    private bool startPlacing = false;

    public Button placeObjectButton;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).position[0] < 570)
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
        Button btn = placeObjectButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        startPlacing = !startPlacing;
        placeObject.isOn = !placeObject.isOn;
    }
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }
        else
        {
            Debug.Log("position is" + Input.GetTouch(0).position);
        }
        if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if(startPlacing == true)
            {
                if(spwanedObject == null)
                {
                    spwanedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                }
                else
                {
                    spwanedObject.transform.position = hitPose.position;
                }
            }
        }
        if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.Face))
        {
            Debug.Log("///////////////yes///////////////////////");
        }
    }
}
