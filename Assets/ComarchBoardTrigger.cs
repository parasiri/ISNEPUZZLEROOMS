using UnityEngine;

public class ComarchBoardTrigger : MonoBehaviour
{
    public LogicGatePuzzleManager puzzleManager;

    void OnMouseDown()
    {
        if (puzzleManager != null)
        {
            puzzleManager.StartPhase2();
        }
    }
}
