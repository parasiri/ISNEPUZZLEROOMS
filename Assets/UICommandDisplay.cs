using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UICommandDisplay : MonoBehaviour
{
    public static UICommandDisplay Instance;

    public TextMeshProUGUI commandText;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateCommandText(List<string> commands)
    {
        commandText.text = "Commands:\n";

        foreach (string cmd in commands)
        {
            commandText.text += cmd + "\n";
        }
    }
}
