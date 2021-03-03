using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startState : MonoBehaviour
{
    public Canvas First;
    public Canvas Second;
    public Canvas Third;
    // Start is called before the first frame update
    void Start()
    {
        First.gameObject.SetActive(false);
        Second.gameObject.SetActive(false);
        Third.gameObject.SetActive(false);

        var allCanvas = FindObjectsOfType<Canvas>();
        foreach (Canvas part in allCanvas)
        {
            if (part != First || part != Second || part != Third)
            {
                part.gameObject.SetActive(true);
            }
        }
    }
}
