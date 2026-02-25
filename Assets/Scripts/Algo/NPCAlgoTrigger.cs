using UnityEngine;

public class NPCAlgoTrigger : MonoBehaviour
{
    public AlgoDialogue dialogue;

    private void OnMouseDown()
    {
        dialogue.OpenPanel();
    }
}
