using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionOnMachine : MonoBehaviour
{
    public Button trigger;
    // Start is called before the first frame update
    void Start()
    {
        trigger.onClick.AddListener(WhenClicked);
    }
    void WhenClicked()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
