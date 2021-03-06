using UnityEngine;
using UnityEngine.UI;

public class AddingMachines : MonoBehaviour
{
    public Button add;
    public Button Enter;
    public Button DeleteButton;
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
        Enter.gameObject.SetActive(!nameActive);
        NotifyMessage.gameObject.SetActive(!nameActive);
        DeleteButton.gameObject.SetActive(!nameActive);
        NotifyMessage.text = "";
        machineNameInput.text = "";
    }
    void onDeleteClick()
    {
        string machineName = machineNameInput.text;
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/{0}" + machineName))
        {
            displayTextForSecond("Notify Message", "Machine don't Exist!", 2.0f);
        }
        else
        {
            System.IO.Directory.Delete(Application.persistentDataPath + "/{0}" + machineName);
            displayTextForSecond("Notify Message", "Machine deleted!", 2.0f);
        }
    }
    
    void onEnterClick()
    {
        string machineName = machineNameInput.text;
        if(System.IO.Directory.Exists(Application.persistentDataPath + "/{0}" + machineName))
        {
            displayTextForSecond("Notify Message", "Machine Alreay Exist!", 2.0f);
            //Debug.Log("exist//////////////////");
        }
        else
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/{0}" + machineName);
            displayTextForSecond("Notify Message", "Machine Added!", 2.0f);
            //Debug.Log("create//////////////////");
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
