using UnityEngine;

public class LogicGateFrame : MonoBehaviour
{
    [Header("Frame Index (1 or 2)")]
    public int frameIndex;

    [Header("Puzzle Manager")]
    public LogicGatePuzzleManager puzzleManager;

    void OnMouseDown()
    {
        if (puzzleManager == null)
        {
            Debug.LogError("PuzzleManager not assigned on " + gameObject.name);
            return;
        }

        puzzleManager.SelectFrame(frameIndex);
    }
}
