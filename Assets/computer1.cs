using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class computer1 : MonoBehaviour
{
    public NetworkDialogue networkDialogue;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI resultText;
    public TMP_InputField inputField;
    public Button submitButton;
    public Button closeButton;
    public computer2 computer2;

    private bool activated = false;
    private string networkAddress;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        close();
        if (resultText != null)
        {
            resultText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (activated)
        {
            // Calculate network address when opening
            CalculateNetworkAddress();
            dialoguePanel.SetActive(true);
        }
        else
        {
            Debug.Log("locked");
        }
    }

    void CalculateNetworkAddress()
    {
        // Extract the first 3 octets from server IP to get network address
        string[] parts = networkDialogue.ipaddress.Split('.');
        if (parts.Length == 4)
        {
            networkAddress = parts[0] + "." + parts[1] + "." + parts[2] + ".0";
            dialogueText.text = "What is the network address? (Server IP: " + networkDialogue.ipaddress + ", Subnet: /24)";
        }
    }

    public void CheckAnswer()
    {
        if (inputField.text == networkAddress)
        {
            Debug.Log("Correct Answer: " + networkAddress);
            resultText.gameObject.SetActive(true);
            resultText.text = "Correct! Network address is " + networkAddress + ". You may proceed to the next computer.";
            computer2.open();
        }
        else
        {
            Debug.Log("Wrong Answer. Expected: " + networkAddress);
            resultText.gameObject.SetActive(true);
            resultText.text = "Incorrect. Try again.";
        }
    }

    public void open()
    {
        activated = true;
    }

    public void close()
    {
        dialoguePanel.SetActive(false);
    }
}