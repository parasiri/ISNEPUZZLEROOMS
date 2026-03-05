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

    public void AddForward05()
    {
        CodeWalkerController.Instance.AddCommand(new MoveForwardCommand(0.5f));
    }

    public void AddForward1()
    {
        CodeWalkerController.Instance.AddCommand(new MoveForwardCommand(1f));
    }

    public void AddForward2()
    {
        CodeWalkerController.Instance.AddCommand(new MoveForwardCommand(2f));
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
