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
        CodeWalkerController.Instance.AddCommand(new MoveForwardCommand());
    }

    public void TurnRight45()
    {
        CodeWalkerController.Instance.AddCommand(new TurnCommand(45));
    }

    public void TurnRight90()
    {
        CodeWalkerController.Instance.AddCommand(new TurnCommand(90));
    }

    public void TurnLeft45()
    {
        CodeWalkerController.Instance.AddCommand(new TurnCommand(-45));
    }

    public void TurnLeft90()
    {
        CodeWalkerController.Instance.AddCommand(new TurnCommand(-90));
    }
}
