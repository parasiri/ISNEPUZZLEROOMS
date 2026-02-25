using UnityEngine;
using TMPro;
using System.Collections;

public class AlgoDialogue : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Timer System")]
    public CountdownTimer countdownTimer;

    // ================== STATE ==================
    private bool timerStarted = false;
    private bool hasPlayed = false;

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    private Coroutine activeCoroutine;

    // ================== INTRO DIALOGUE ==================
    private string[] dialogueLines =
    {
        "Welcome to the Algorithm Room!",
        "In this room, you will learn how algorithms organize data.",
        "An algorithm is a step-by-step process used to solve a problem.",
        "I have hidden several numbers inside the books.",
        "They might be at the top left near me...",
        "Hmm... and I really like CYAN and YELLOW!",
        "Find those 5 numbers and use them to build the correct tree.",
        "Be careful — the structure of the tree matters.",
        "Once the timer starts, the challenge begins.",
        "You can DOUBLE CLICK on the shelf to see it closer",
        "Good luck!"
    };

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    // ================== OPEN ==================
    public void OpenPanel()
    {
        if (hasPlayed) return;

        hasPlayed = true;
        currentIndex = 0;

        dialoguePanel.SetActive(true);
        StartDialogueCoroutine(PlayDialogue());
    }

    void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (Input.GetMouseButtonDown(0) && isTyping)
        {
            skipTyping = true;
        }
    }

    // ================== COROUTINE CONTROL ==================
    void StartDialogueCoroutine(IEnumerator routine)
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(routine);
    }

    // ================== INTRO FLOW ==================
    IEnumerator PlayDialogue()
    {
        while (currentIndex < dialogueLines.Length)
        {
            yield return StartCoroutine(TypeLine(dialogueLines[currentIndex]));

            // ⭐ ประโยคสุดท้าย → เริ่ม Timer ทันที
            if (currentIndex == dialogueLines.Length - 1)
            {
                StartTimerIfNeeded();
                CloseDialogue();
                yield break;
            }

            // รอคลิกเพื่อไปประโยคถัดไป
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

    // ================== TIMER ==================
    void StartTimerIfNeeded()
    {
        if (timerStarted) return;

        timerStarted = true;
        Debug.Log("🔥 START TIMER (ALGO)");

        if (countdownTimer != null)
        {
            countdownTimer.gameObject.SetActive(true);
            countdownTimer.ShowTutorialTimer();
            countdownTimer.timerText.gameObject.SetActive(true); // บังคับ
            countdownTimer.StartCountdownTutorial();
        }
    }

    // ================== CLOSE ==================
    void CloseDialogue()
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = null;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    // =====================================================
    // =============== AFTER INTRO DIALOGUES =================
    // =====================================================

    public void ContinueAfterCorrectAnswer()
    {
        ForceStop();
        dialoguePanel.SetActive(true);
        activeCoroutine = StartCoroutine(PlayAfterCorrectAnswer());
    }

    IEnumerator PlayAfterCorrectAnswer()
    {
        yield return StartCoroutine(TypeLine("You are SOOO GOOOOD!!!"));
        yield return new WaitForSeconds(0.6f);
        CloseDialogue();
    }

    public void ShowWrongAnswer()
    {
        ForceStop();
        dialoguePanel.SetActive(true);
        activeCoroutine = StartCoroutine(PlayWrongAnswer());
    }

    IEnumerator PlayWrongAnswer()
    {
        yield return StartCoroutine(TypeLine("Hmm... something is not quite right."));
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(TypeLine("Check the tree structure carefully and try again."));
        yield return new WaitForSeconds(1.2f);

        CloseDialogue();
    }

    public void ShowFoundNumberDialogue(int number)
    {
        ForceStop();
        dialoguePanel.SetActive(true);
        activeCoroutine = StartCoroutine(PlayFoundNumber(number));
    }

    IEnumerator PlayFoundNumber(int number)
    {
        yield return StartCoroutine(TypeLine($"You found Number : {number}"));
        yield return new WaitForSeconds(1.2f);
        CloseDialogue();
    }

    public void ShowTreeInstruction(AlgoTreeType type)
    {
        StartCoroutine(DelayedTreeInstruction(type));
    }

    IEnumerator DelayedTreeInstruction(AlgoTreeType type)
    {
        yield return new WaitForSeconds(2f);

        ForceStop();
        dialoguePanel.SetActive(true);
        activeCoroutine = StartCoroutine(PlayTreeInstruction(type));
    }

    IEnumerator PlayTreeInstruction(AlgoTreeType type)
    {
        string line =
            type == AlgoTreeType.BST
            ? "Now, arrange them into a Binary Search Tree."
            : "Now, arrange them into a Balanced Binary Tree.";

        yield return StartCoroutine(TypeLine(line));
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(
            TypeLine("Go arrange the numbers on the board at the back of the room!")
        );
        yield return new WaitForSeconds(1.2f);

        CloseDialogue();
    }


    void ForceStop()
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = null;
        isTyping = false;
        skipTyping = false;
    }
}
