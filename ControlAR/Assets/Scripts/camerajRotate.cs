using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class camerajRotate : MonoBehaviour
{
    public Button reset;

    public Text focusIndicator;
    public Button startFocus;
    private Vector3 center = new Vector3(0, 0, 0);
    private Vector3 ball = new Vector3(0, 0, 0);
    private Vector3 difference = new Vector3(0, 0, 0);
    private float angle;
    private float angleUp;
    private bool focus = false;

    Vector3 touchAngle = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        reset.onClick.AddListener(onResetClick);
        startFocus.onClick.AddListener(onFocusClick);
    }

    void onFocusClick()
    {
        focus = !focus;
        focusIndicator.text = ("Focus Mode: " + focus);
    }
    void onResetClick()
    {
        //StartCoroutine(smoothRotate(center - difference, difference, 1f));

        StartCoroutine(smoothRotate(center - difference, new Vector3(0,0,0), 1f));
        StartCoroutine(smoothMove(transform.position, new Vector3(0, 0, -5), 1f));
        center = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 0, -5);
    }
    IEnumerator smoothMove(Vector3 pos1, Vector3 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        transform.position = pos2;
    }
    IEnumerator smoothRotate(Vector3 start,Vector3 end, float duration)
    {
        Vector3 distance = end - start;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.LookAt(start + (distance * t / duration));
            yield return 0;
        }
        transform.LookAt(end);
    }
    // Update is called once per frame
    float distance = 0;
    Vector2 middlePoint = new Vector2(0, 0);
    void Update()
    {
        Vector3 newpoint = GameObject.Find("plotter").GetComponent<LinePlotter>().ballPoint;
        float scale = GameObject.Find("plotter").GetComponent<LinePlotter>().plotScale;
        if (center != newpoint && focus == true)
        {
            StopAllCoroutines();
            difference = newpoint - center;
            transform.LookAt(center);
            StartCoroutine(smoothRotate(center, newpoint, 1f));
        }
        if(focus == true)
        {
            center = newpoint;
        }
        if(Input.touchCount >= 2)
        {
            Vector2 touch0, touch1;
            float newDistance;
            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;
            newDistance = Vector2.Distance(touch0, touch1) / 400;
            Vector2 newMiddlePoint = (touch0 + touch1) / 2;
            if((Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) && Mathf.Abs(newDistance - distance) < 0.05 && Vector2.Distance(newMiddlePoint,middlePoint) > 10)
            {
                transform.position = transform.position - transform.right * ((newMiddlePoint.x - middlePoint.x)/400) * Vector3.Distance(transform.position, center);
                transform.position = transform.position - transform.up * ((newMiddlePoint.y - middlePoint.y)/ 400) * Vector3.Distance(transform.position, center);
            }
            if((Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) && Mathf.Abs(newDistance - distance) >= 0.05)
            {
                Vector3 movement = transform.forward*(newDistance - distance) * Vector3.Distance(transform.position, center) / 10;
                if(Vector3.SqrMagnitude(movement) < 1 && Vector3.Distance((transform.position + movement), center) > scale/10)
                {
                    transform.position += movement;
                }
            }
            else
            {
                distance = newDistance;
                middlePoint = newMiddlePoint;
            }
        }
        else if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                if (Input.GetTouch(0).phase != TouchPhase.Began)
                {
                    Vector2 touchRotatePosition = Input.GetTouch(0).position;
                    Vector3 newTouchAngle = new Vector3(touchRotatePosition.x, touchRotatePosition.y, 0); 
                    transform.RotateAround(center, transform.right, ( touchAngle.y - newTouchAngle.y) / 2);
                    transform.RotateAround(center, transform.up, ( newTouchAngle.x - touchAngle.x) / 2);
                    touchAngle = newTouchAngle;
                }
                else
                {
                    touchAngle = Input.GetTouch(0).position;
                }
            }
        }
    }
}
