using UnityEngine;

public class PuzzleStateManager : MonoBehaviour
{
    public static PuzzleStateManager Instance;

    private bool puzzleSolved = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPuzzleSolved()
    {
        puzzleSolved = true;
    }

    public bool IsPuzzleSolved()
    {
        return puzzleSolved;
    }

    public void ResetPuzzle()
    {
        puzzleSolved = false;
    }
}