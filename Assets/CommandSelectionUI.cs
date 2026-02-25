using UnityEngine;

public class CommandSelectionUI : MonoBehaviour
{
    public static CommandSelectionUI Instance;

    public GameObject panel;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void AddForward()
    {
        CodeWalkerController.Instance.AddCommand("Forward");
    }

    public void AddLeft()
    {
        CodeWalkerController.Instance.AddCommand("Left");
    }

    public void AddRight()
    {
        CodeWalkerController.Instance.AddCommand("Right");
    }
}
