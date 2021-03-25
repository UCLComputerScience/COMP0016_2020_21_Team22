using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMachineForData : MonoBehaviour
{
    public GameObject canvasHolder;
    public GameObject buttonHolder;
    void Start()
    {
        var allMachines = System.IO.Directory.GetDirectories(Application.persistentDataPath);
        GameObject selectionCanvas = Instantiate(canvasHolder);
        selectionCanvas.name = "Selection Canvas";
        int counter = 0;
        foreach (string machine in allMachines)
        {
            string machineName = getDifferenceAtEnd(machine, Application.persistentDataPath + "/{0}");
            int column = counter % 2 + 1;
            int row = counter / 2 + 1;
            GameObject machineButtonsHolder = Instantiate(buttonHolder, new Vector3(column * 300 - 90, 1440 - row * 200, 0), Quaternion.identity);
            machineButtonsHolder.transform.SetParent(selectionCanvas.transform);

            Button machineButtons = machineButtonsHolder.GetComponent<Button>();
            if (machineButtons != null)
            {
                machineButtons.transform.name = machine;
                machineButtons.onClick.AddListener(delegate { onMachineSelected(machineName); });
            }
            Text MachineName = machineButtons.GetComponentInChildren<Text>();
            if (MachineName != null)
            {
                MachineName.text = machineName;
            }

            counter += 1;
        }


    }
    private void onMachineSelected(string machineName)
    {

        GameObject.Destroy(GameObject.Find("Selection Canvas"));
        if (machineName != null && GameObject.Find(machineName) == null)
        {
            GameObject download = new GameObject();
            string link = System.IO.File.ReadAllText(Application.persistentDataPath + "/{0}" + machineName + "/link.txt");
            download.AddComponent<CSVDownloader>().fileName = link;
            download.GetComponent<CSVDownloader>().path = machineName;
            GameObject.Find("plotter").GetComponent<LinePlotter>().inputfile = machineName;

            GameObject goBack = new GameObject();
            goBack.AddComponent<goBackByGesture>().sceneNum = 0;

        }
    }

    private string getDifferenceAtEnd(string a, string b)
    {
        string c;
        c = a.Substring(b.Length);
        return c;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            onMachineSelected(null);
        }
    }
}
