using UnityEngine;

public class WalkerClickHandler : MonoBehaviour
{
    void OnMouseDown()
    {
        if (CommandSelectionUI.Instance != null)
        {
            CommandSelectionUI.Instance.OpenPanel();
        }
        else
        {
            Debug.LogError("CommandSelectionUI Instance is NULL!");
        }
    }

}
