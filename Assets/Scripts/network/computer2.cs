using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class computer2 : MonoBehaviour
{
    public NetworkDialogue networkDialogue;
    private string myipaddress;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI resultText;
    public TMP_InputField inputField;
    public Button submitButton;
    public Button closeButton;
    private bool activated = false;

    private bool currentStep = false;
    private string networkPrefix;

    void Start()
    {
        close();
        if (resultText != null)
        {
            resultText.gameObject.SetActive(false);
        }
    }


    void Update()
    {

    }

    public void OnMouseDown()
    {
        if (activated)
        {
            // Calculate network prefix when opening
            CalculateNetworkPrefix();
            dialoguePanel.SetActive(true);
            ShowQuestion();
        }
        else
        {
            Debug.Log("locked");
        }
    }

    void CalculateNetworkPrefix()
    {
        // Extract the first 3 octets from server IP
        string[] parts = networkDialogue.ipaddress.Split('.');
        if (parts.Length == 4)
        {
            networkPrefix = parts[0] + "." + parts[1] + "." + parts[2] + ".";
        }
    }

    void ShowQuestion()
    {
        if (!currentStep)
        {
            dialogueText.text = "Assign an IP address to this PC.";
        }
        else
        {
            dialogueText.text = "Enter command to ping the server (Server IP: " + networkDialogue.ipaddress + "):";
        }
    }

    public void CheckAnswer()
    {
        if (!currentStep)
        {
            // Check if IP address is in correct format: same network as server
            if (inputField.text.StartsWith(networkPrefix))
            {
                string[] parts = inputField.text.Split('.');
                if (parts.Length == 4 && int.TryParse(parts[3], out int lastOctet))
                {
                    if (lastOctet >= 1 && lastOctet <= 254 && inputField.text != networkDialogue.ipaddress)
                    {
                        myipaddress = inputField.text;
                        Debug.Log("Correct IP Address: " + myipaddress);
                        resultText.gameObject.SetActive(true);
                        resultText.text = "Correct! IP address assigned: " + myipaddress;
                        inputField.text = "";
                        currentStep = true;
                        ShowQuestion();
                    }
                    else
                    {
                        resultText.gameObject.SetActive(true);
                        resultText.text = "Incorrect. IP shouldn't be the same as server IP and must be 1-254.";
                    }
                }
                else
                {
                    resultText.gameObject.SetActive(true);
                    resultText.text = "Incorrect IP format.";
                }
            }
            else
            {
                resultText.gameObject.SetActive(true);
                resultText.text = "Incorrect. IP must start with " + networkPrefix;
            }
        }
        else
        {
            if (inputField.text == "ping " + networkDialogue.ipaddress)
            {
                Debug.Log("Correct Answer");
                resultText.gameObject.SetActive(true);
                resultText.text = "Correct! You have successfully pinged the server.";
                networkDialogue.Outtro();
                close();
            }
            else
            {
                Debug.Log("Wrong Answer");
                resultText.gameObject.SetActive(true);
                resultText.text = "Incorrect command. Use: ping " + networkDialogue.ipaddress;
            }
        }
    }


    public void open()
    {
        Debug.Log("activated");
        activated = true;
    }

    public void close()
    {
        dialoguePanel.SetActive(false);
        currentStep = false; // Reset when closing
    }
}