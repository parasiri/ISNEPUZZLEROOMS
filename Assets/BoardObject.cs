using UnityEngine;

public class BoardInteraction : MonoBehaviour
{
    public PlayfairBoardUI boardUI;

    void OnMouseDown()
    {
        if (NetsecPuzzleManager.Instance.PuzzleStarted)
        {
            boardUI.OpenBoard();
        }
        else
        {
            Debug.Log("Puzzle not started yet.");
        }
    }
}
