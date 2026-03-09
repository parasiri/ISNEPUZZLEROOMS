using UnityEngine;
using TMPro;
using System.Collections;

public class ComacrhDialogue : MonoBehaviour, IRoomDialogue
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Timer System")]
    public CountdownTimer countdownTimer;

    [Header("Puzzle")]
    public LogicGatePuzzleManager puzzleManager;

    [Header("Optional UI")]
    public GameObject closeButton;   // ⭐ ปุ่ม X

    // ================== INTRO DIALOGUE ==================

    private string[] dialogueLines =
    {
        "Welcome to the Computer Architecture Room.",
        "This room will test your understanding of logic gates.",
        "Logic gates are the foundation of all digital systems.",
        "Each gate receives binary inputs — either 0 or 1.",
        "Based on its logic, it produces a specific output.",
        "Different gates behave differently, even with the same inputs.",
        "In front of you, you will see logic gate symbols.",
        "Your task is to identify each gate correctly.",
        "Choose the correct answer before time runs out.",
        "Focus carefully...",
        "Let’s begin the logic challenge!"
    };
    public void ShowDoorLockedMessage()
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = "You can't leave this room yet.\nSolve the puzzle first!";
    }

    public void ShowMessage(string msg)
    {
        Debug.Log(msg);
    }

    private int currentIndex = 0;
    private bool hasPlayed = false;
    private bool timerStarted = false;

    private bool isTyping = false;
    private bool skipTyping = false;

    private Coroutine activeCoroutine;

    // ================== START ==================

    void Start()
    {
        dialoguePanel.SetActive(false);

        if (closeButton != null)
            closeButton.SetActive(true);
    }

    // ================== OPEN NPC ==================

    public void OpenPanel()
    {
        if (hasPlayed) return;

        hasPlayed = true;
        currentIndex = 0;

        dialoguePanel.SetActive(true);
        StartDialogueCoroutine(PlayDialogue());
    }

    // ================== UPDATE ==================

    void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
                skipTyping = true;
        }
    }

    // ================== COROUTINE CONTROL ==================

    void StartDialogueCoroutine(IEnumerator routine)
    {
        ForceStopDialogue();
        activeCoroutine = StartCoroutine(routine);
    }

    // ================== INTRO DIALOGUE ==================

    IEnumerator PlayDialogue()
    {
        while (currentIndex < dialogueLines.Length)
        {
            yield return StartCoroutine(TypeLine(dialogueLines[currentIndex]));

            // ⭐ ประโยคสุดท้าย → auto start
            if (currentIndex == dialogueLines.Length - 1)
            {
                StartTimerAndPuzzle();
                CloseDialogue();
                yield break;
            }

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            currentIndex++;
        }
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        isTyping = true;
        skipTyping = false;

        foreach (char c in line)
        {
            if (skipTyping)
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    // ================== TIMER + PUZZLE ==================

    void StartTimerAndPuzzle()
    {
        if (timerStarted) return;
        timerStarted = true;

        Debug.Log("START COMARCH TIMER + PUZZLE");

        if (countdownTimer != null)
        {
            countdownTimer.gameObject.SetActive(true);
            countdownTimer.ShowTutorialTimer();
            countdownTimer.StartCountdownTutorial();
        }

        if (puzzleManager != null)
        {
            puzzleManager.StartPuzzle();
        }
    }

    // ================== CLOSE BUTTON ==================

    // 🔘 เรียกจากปุ่ม X
    public void OnCloseButtonPressed()
    {
        // ถ้าผู้เล่นกดปิดก่อน intro จบ → เริ่มเกมทันที
        StartTimerAndPuzzle();
        CloseDialogue();
    }

    void CloseDialogue()
    {
        ForceStopDialogue();
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    // ================== CORRECT (ต่อเฟรม) ==================

    public void ShowCorrectFeedback()
    {
        ForceStopDialogue();
        dialoguePanel.SetActive(true);
        activeCoroutine = StartCoroutine(PlayCorrectFeedback());
    }

    IEnumerator PlayCorrectFeedback()
    {
        yield return StartCoroutine(TypeLine("Correct!"));
        yield return new WaitForSeconds(0.8f);
        CloseDialogue();
    }

    // ================== FINAL ==================

    public void ContinueAfterCorrectAnswer()
    {
        ForceStopDialogue();
        dialoguePanel.SetActive(true);
        activeCoroutine = StartCoroutine(PlayAfterCorrectAnswer());
    }

    IEnumerator PlayAfterCorrectAnswer()
    {
        yield return StartCoroutine(TypeLine("Correct."));
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(TypeLine("You understand how this logic gate works."));
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(TypeLine("This logic is used inside real processors."));
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(TypeLine("The path forward is now open."));
        yield return new WaitForSeconds(0.4f);

        CloseDialogue();
    }

    public void ShowPhase2Intro()
    {
        ForceStopDialogue();
        dialoguePanel.SetActive(true);
        activeCoroutine = StartCoroutine(PlayPhase2Intro());
    }

    IEnumerator PlayPhase2Intro()
    {
        yield return StartCoroutine(TypeLine("Excellent work."));
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(TypeLine("But there is still one final puzzle."));
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(TypeLine("Check the board behind me."));
        yield return new WaitForSeconds(0.4f);

        CloseDialogue();
    }

    public void ShowFinalSuccess()
    {
        ForceStopDialogue();
        dialoguePanel.SetActive(true);
        activeCoroutine = StartCoroutine(PlayFinalSuccess());
    }

    IEnumerator PlayFinalSuccess()
    {
        yield return StartCoroutine(TypeLine("Impressive."));
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(TypeLine("You have mastered logic gates."));
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(TypeLine("You may proceed to the next room."));
        yield return new WaitForSeconds(0.4f);

        CloseDialogue();
    }



    // ================== FORCE STOP ==================

    public void ForceStopDialogue()
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = null;
        isTyping = false;
        skipTyping = false;
    }
}
