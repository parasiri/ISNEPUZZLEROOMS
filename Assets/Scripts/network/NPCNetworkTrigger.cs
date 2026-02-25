using UnityEngine;

public class NPCNetworkTrigger : MonoBehaviour
{
    public NetworkDialogue dialogue;

    private void OnMouseDown()
    {
        dialogue.OpenPanel();
    }
}