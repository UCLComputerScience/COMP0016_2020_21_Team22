using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public Button showConfiguration;

    public Canvas toShow;

    // Start is called before the first frame update
    void Start()
    {
        showConfiguration.onClick.AddListener(onShowAxesClick);
    }
    void onShowAxesClick()
    {
        bool showing = toShow.gameObject.activeSelf;
        toShow.gameObject.SetActive(!showing);
    }
}
