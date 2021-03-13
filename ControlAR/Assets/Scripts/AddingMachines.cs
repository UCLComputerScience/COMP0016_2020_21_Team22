using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class AddingMachines : MonoBehaviour
{
    public Button add;
    public Button Enter;
    public Button DeleteButton;
    public InputField azureLink;
    public InputField machineNameInput;
    public Text NotifyMessage;

    void Start()
    {
        add.onClick.AddListener(OnAddClick);
        Enter.onClick.AddListener(onEnterClick);
        DeleteButton.onClick.AddListener(onDeleteClick);
    }

    
    void OnAddClick()
    {
        bool nameActive = machineNameInput.IsActive();
        machineNameInput.gameObject.SetActive(!nameActive);
        azureLink.gameObject.SetActive(!nameActive);
        Enter.gameObject.SetActive(!nameActive);
        NotifyMessage.gameObject.SetActive(!nameActive);
        DeleteButton.gameObject.SetActive(!nameActive);
        NotifyMessage.text = "";
        machineNameInput.text = "";
    }
    void onDeleteClick()
    {
        string machineName = machineNameInput.text;
        string path = Application.persistentDataPath + "/{0}" + machineName;
        if (!System.IO.Directory.Exists(path))
        {
            displayTextForSecond("Notify Message", "Machine don't Exist!", 2.0f);
        }
        else
        {
            deleteAllFiles(path);
            System.IO.Directory.Delete(path);
            displayTextForSecond("Notify Message", "Machine deleted!", 2.0f);
        }
    }
    
    void onEnterClick()
    {
        string machineName = machineNameInput.text;
        string linkName = azureLink.text;
        if(System.IO.Directory.Exists(Application.persistentDataPath + "/{0}" + machineName))
        {
            displayTextForSecond("Notify Message", "Machine Alreay Exist!", 2.0f);
            //Debug.Log("exist//////////////////");
        }
        else
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/{0}" + machineName);
            string path = Application.persistentDataPath + "/{0}" + machineName + "/link.txt";
            try
            {
                System.IO.File.Create(path);
                System.IO.File.WriteAllText(path, linkName);
            }catch(IOException e)
            {

            }
            displayTextForSecond("Notify Message", "Machine Added!", 2.0f);
            //Debug.Log("create//////////////////");
        }
    }
    private void deleteAllFiles(string path)
    {
        var files = System.IO.Directory.GetFiles(path);
        foreach(string x in files)
        {
            if (File.Exists(x))
            {
                File.Delete(x);
            }
        }
    }
    void displayTextForSecond(string Fieldname, string text, float time)
    {
        //GameObject newTextField = new GameObject(Fieldname);
        //newTextField.transform.SetParent(transform);

        //Text display = newTextField.AddComponent<Text>();
        //newTextField.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
        //newTextField.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
        //newTextField.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(150, -173, 0);
        //display.fontSize = 100;
        //display.text = text;
        ////GameObject.Destroy(newTextField, time);

        NotifyMessage.text = text;
    }
}
