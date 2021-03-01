using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class datapath : MonoBehaviour
{
    public Text path;
    // Start is called before the first frame update
    void Start()
    {
        path.text = Application.persistentDataPath;
    }
}
