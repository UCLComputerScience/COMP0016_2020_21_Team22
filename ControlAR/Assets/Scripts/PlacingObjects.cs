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
    private int counter = 0;
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
        Button btn = placeObjectButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if(startPlacing == false)
        {
            startPlacing = true;
            counter += 1;
            placeObject.isOn = true;
        }
        else
        {
            startPlacing = false;
            placeObject.isOn = false;
        }
    }
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
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
    }
}
