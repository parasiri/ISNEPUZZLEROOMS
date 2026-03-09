using UnityEngine;

public class OOPPuzzleManager : MonoBehaviour
{
    public static OOPPuzzleManager Instance;

    [Header("Puzzle Objects")]
    public GameObject walkerObject;
    public GameObject successPanel;
    public GameObject failPanel;

    private bool puzzleStarted = false;
    private bool puzzleCompleted = false;

    public OOPDialogue oopDialogue;

    public CountdownTimer countdownTimer;
    private bool isSaved = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (successPanel != null)
            successPanel.SetActive(false);

        if (failPanel != null)
            failPanel.SetActive(false);

        //LockWalker();
    }

    // ================= START PUZZLE =================

    public void StartPuzzle()
    {
        if (puzzleStarted) return;

        puzzleStarted = true;
        puzzleCompleted = false;

        Debug.Log("OOP Puzzle Started");

        UnlockWalker();
    }

    // ================= COMPLETE =================

    public void PuzzleCompleted()
    {
        if (puzzleCompleted) return;

        puzzleCompleted = true;

        // ปลดล็อคประตู
        if (PuzzleStateManager.Instance != null)
        {
            PuzzleStateManager.Instance.SetPuzzleSolved();
        }

        Debug.Log("OOP Puzzle Completed!");

        // ===== STOP TIMER =====
        if (!isSaved)
        {
            isSaved = true;

            countdownTimer.StopCountdown();

            float usedTime = countdownTimer.GetTimeUsed();

            if (PlayerDataManager.Instance != null)
            {
                PlayerDataManager.Instance.SaveRoomTime(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                    usedTime
                );
            }
        }

        if (oopDialogue != null)
            oopDialogue.PlaySuccessDialogue();
    }

    // ================= FAIL (TIME OUT) =================

    public void PuzzleFailed()
    {
        if (puzzleCompleted) return;

        Debug.Log("OOP Puzzle Failed!");

        if (failPanel != null)
            failPanel.SetActive(true);

        LockWalker();
    }

    // ================= LOCK SYSTEM =================

    void LockWalker()
    {
        if (walkerObject != null)
            walkerObject.SetActive(false);
    }

    void UnlockWalker()
    {
        if (walkerObject != null)
            walkerObject.SetActive(true);
    }

    // ================= RESET PUZZLE =================

    public void ResetPuzzle()
    {
        puzzleStarted = false;
        puzzleCompleted = false;

        if (successPanel != null)
            successPanel.SetActive(false);

        if (failPanel != null)
            failPanel.SetActive(false);

        LockWalker();
    }

    public bool IsPuzzleStarted()
    {
        return puzzleStarted;
    }

    public bool IsPuzzleCompleted()
    {
        return puzzleCompleted;
    }
}
