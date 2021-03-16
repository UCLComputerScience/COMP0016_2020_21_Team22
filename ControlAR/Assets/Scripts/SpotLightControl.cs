using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpotLightControl : MonoBehaviour
{
    public Material lighted;
    public Material dark;
    public Button lightSwitch;
    private bool isLIghtOn = false;
    void Start()
    {
        lightSwitch.onClick.AddListener(onLightSwitched);
    }

    void onLightSwitched()
    {
        isLIghtOn = !isLIghtOn;
        if (!isLIghtOn)
        {
            try
            {
                GameObject machine = GameObject.Find("test machine");
                machine.GetComponent<MeshRenderer>().material = lighted;
            }
            catch (NullReferenceException)
            {

            }
        }
        else
        {
            try
            {
                GameObject machine = GameObject.Find("test machine");
                machine.GetComponent<MeshRenderer>().material = dark;
            }
            catch (NullReferenceException)
            {

            }
        }

    }
}
