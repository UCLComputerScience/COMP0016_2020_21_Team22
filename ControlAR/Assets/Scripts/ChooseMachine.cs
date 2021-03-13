using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class ChooseMachine : MonoBehaviour
{
    public Button selectMachine;
    public GameObject canvasHolder;
    public GameObject buttonHolder;


    void Start()
    {
        selectMachine.onClick.AddListener(onSelectClick);
    }

    void onSelectClick()
    {
        var allMachines = System.IO.Directory.GetDirectories(Application.persistentDataPath);
        GameObject selectionCanvas = Instantiate(canvasHolder);
        selectionCanvas.name = "machine selection page";
        int counter = 0;
        foreach (string machine in allMachines)
        {
            string machineName = getDifferenceAtEnd(machine, Application.persistentDataPath+"/{0}");
            int column = counter % 2 + 1;
            int row = counter / 2 + 1;
            GameObject machineButtonsHolder = Instantiate(buttonHolder, new Vector3(column * 300 - 90 , 1440 - row * 200, 0), Quaternion.identity);
            machineButtonsHolder.transform.SetParent(selectionCanvas.transform);

            Button machineButtons = machineButtonsHolder.GetComponent<Button>();
            if (machineButtons != null)
            {
                machineButtons.transform.name = machineName;
                machineButtons.onClick.AddListener(delegate { onMachineSelected(machineName); });
            }
            else
            {
                Debug.Log("AHOH");
            }
            Text MachineName = machineButtons.GetComponentInChildren<Text>();
            if (MachineName != null)
            {
                MachineName.text = machineName;
                Debug.Log("AHOH");
            }

            counter += 1;
            Debug.Log(machine);
        }
    }

    private string getDifferenceAtEnd(string a, string b)
    {
        string c;
        c = a.Substring(b.Length);
        return c;
    }
    void onMachineSelected(string machineName)
    {
        GameObject.Destroy(GameObject.Find("machine selection page"));
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
