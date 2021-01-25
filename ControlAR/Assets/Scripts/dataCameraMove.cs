using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dataCameraMove : MonoBehaviour
{

    public Button up;
    public Button down;
    public Button left;
    public Button right;
    public Button forward;
    public Button back;

    // Start is called before the first frame update
    void Start()
    {
        Button goUp = up.GetComponent<Button>();
        goUp.onClick.AddListener(onUpClick);
        Button goDown = up.GetComponent<Button>();
        goDown.onClick.AddListener(onDownClick);
        Button goLeft = up.GetComponent<Button>();
        goLeft.onClick.AddListener(onLeftClick);
        Button goRight = up.GetComponent<Button>();
        goRight.onClick.AddListener(onRightClick);
        Button goForward = up.GetComponent<Button>();
        goForward.onClick.AddListener(onForwardClick);
        Button goBack = up.GetComponent<Button>();
        goBack.onClick.AddListener(onBackClick);
    }

    // Update is called once per frame
    void onUpClick()
    {
        transform.position = transform.position + new Vector3(0,1,0);
    }
    void onDownClick()
    {
        transform.position = transform.position + new Vector3(0,-1,0);
    }
    void onLeftClick()
    {
        transform.position = transform.position + new Vector3(-1,0,0);
    }
    void onRightClick()
    {
        transform.position = transform.position + new Vector3(1,0,0);
    }
    void onForwardClick()
    {
        transform.position = transform.position + new Vector3(0,0,1);
    }
    void onBackClick()
    {
        transform.position = transform.position + new Vector3(0,0,-1);
    }
}
