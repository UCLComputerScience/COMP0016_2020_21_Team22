using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camerajRotate : MonoBehaviour
{

    public Slider cameraRotation;

    public Button up;
    public Button down;
    public Button left;
    public Button right;
    public Button forward;
    public Button back;

    public Button reset;

    public Text focusIndicator;
    public Button startFocus;
    private Vector3 center = new Vector3(0, 0, 0);
    private Vector3 ball = new Vector3(0, 0, 0);
    private Vector3 difference = new Vector3(0, 0, 0);
    private float angle;
    private bool focus = false;
    // Start is called before the first frame update
    void Start()
    {
        cameraRotation.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        up.onClick.AddListener(onUpClick);
        down.onClick.AddListener(onDownClick);
        left.onClick.AddListener(onLeftClick);
        right.onClick.AddListener(onRightClick);
        forward.onClick.AddListener(onForwardClick);
        back.onClick.AddListener(onBackClick);
        reset.onClick.AddListener(onResetClick);
        startFocus.onClick.AddListener(onFocusClick);
    }

    void onFocusClick()
    {
        focus = !focus;
        focusIndicator.text = ("Focus Mode: " + focus);
    }
    void onUpClick()
    {
        transform.position = transform.position + transform.up;
    }
    void onDownClick()
    {
        transform.position = transform.position - transform.up;
    }
    void onLeftClick()
    {
        transform.position = transform.position - transform.right;
    }
    void onRightClick()
    {
        transform.position = transform.position + transform.right;
    }
    void onForwardClick()
    {
        transform.position = transform.position + transform.forward;
        //Debug.Log(transform.position);
    }
    void onBackClick()
    {
        transform.position = transform.position - transform.forward;
    }

    void ValueChangeCheck()
    {
        transform.RotateAround(center, Vector3.up, cameraRotation.value - angle);
        angle = cameraRotation.value;
    }
    void onResetClick()
    {
        //StartCoroutine(smoothRotate(center - difference, difference, 1f));

        StartCoroutine(smoothRotate(center - difference, new Vector3(0,0,0), 1f));
        StartCoroutine(smoothMove(transform.position, new Vector3(0, 0, -5), 1f));
        center = new Vector3(0, 0, 0);
        cameraRotation.value = 0;
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
    void Update()
    {
        Vector3 newpoint = GameObject.Find("plotter").GetComponent<LinePlotter>().ballPoint;
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
    }
}
