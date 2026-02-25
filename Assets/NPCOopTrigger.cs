using UnityEngine;

public class NPCOOPTrigger : MonoBehaviour
{
    public OOPDialogue dialogue;

    private void OnMouseDown()
    {
        dialogue.OpenPanel();
    }
}