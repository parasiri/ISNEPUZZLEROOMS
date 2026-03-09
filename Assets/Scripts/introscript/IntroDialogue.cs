using UnityEngine;
using TMPro;
using System.Collections;

public class IntroDialogue : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject okButton;

    [Header("Timer System")]
    public CountdownTimer countdownTimer;

    private string[] dialogueLines =
    {
        "Welcome to the ISNE Puzzle Room Again!",
        "This place isn’t just a collection of rooms...",
        "It’s a world of ISNE — where creativity and knowledge come together.",
        "In this game, each room has an NPC waiting for you.",
        "Talk to them, listen carefully…",
        "Because after that, you’ll face a puzzle challenge.",
        "Your goal is simple: solve every puzzle before the timer runs out!",
        "Alright, let’s try a simple tutorial first.",
        "Click on the board and solve the puzzle.",
        "Now you can see the countdown timer at the top right.",
        "You must solve the puzzle on TIME!!",
        "If you can't answer this, we have HINT for you too!",
        "But if your answer is correct, the countdown will stop and the door will open."
    };

    private int currentIndex = 0;
    private bool hasPlayed = false;

    private bool isTyping = false;
    private bool skipTyping = false;

    private Coroutine activeCoroutine = null;


    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }


    // เรียกเมื่อคลิก NPC
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

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
                skipTyping = true;
            else
                currentIndex++;
        }
    }


    // ---------- ฟังก์ชันจัดการ coroutine ป้องกันซ้อน ----------
    void StartDialogueCoroutine(IEnumerator routine)
    {
        // ถ้ามี coroutine เดิมกำลังทำงาน → หยุดก่อน
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(routine);
    }
    // ------------------------------------------------------------


    IEnumerator PlayDialogue()
    {
        while (currentIndex < dialogueLines.Length)
        {
            yield return StartCoroutine(TypeLine(dialogueLines[currentIndex]));

            // เริ่มจับเวลากลางบท
            if (currentIndex == 8)
            {
                if (countdownTimer != null)
                {
                    countdownTimer.ShowTutorialTimer();
                    countdownTimer.StartCountdownTutorial();
                }
            }

            float waitTime = 1.2f;
            float timer = 0f;

            while (timer < waitTime)
            {
                if (!skipTyping && Input.GetMouseButtonDown(0))
                    break;

                timer += Time.deltaTime;
                yield return null;
            }

            currentIndex++;
        }

        ClosePanel();
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


    public void ClosePanel()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }


    // ---------------- Dialogue หลังตอบถูก -------------------

    public void ContinueAfterCorrectAnswer()
    {
        PuzzleStateManager.Instance.SetPuzzleSolved();

        dialoguePanel.SetActive(true);

        // หยุด dialogue เดิมก่อน (สำคัญมาก)
        ClosePanel();
        dialoguePanel.SetActive(true);

        StartDialogueCoroutine(PlayAfterCorrectAnswer());
    }


    IEnumerator PlayAfterCorrectAnswer()
    {
        yield return StartCoroutine(TypeLine("So… are you ready for this adventure?"));
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(TypeLine("Let’s open the door and begin your ISNE journey!"));
        yield return new WaitForSeconds(0.4f);

        ClosePanel();
    }
}
