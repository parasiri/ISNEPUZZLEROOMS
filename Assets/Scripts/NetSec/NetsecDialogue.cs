using UnityEngine;
using TMPro;
using System.Collections;

public class NetsecDialogue : MonoBehaviour, IRoomDialogue
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Timer System")]
    public CountdownTimer countdownTimer;

    [Header("Optional")]
    public GameObject closeButton; // ปุ่ม X หรือ Close

    private string[] dialogueLines =
{
    "Welcome to the Network Security Room.",
    "This room will test your understanding of cryptography.",
    "Specifically, we’ll be using a classic technique called the Playfair Cipher.",
    "Playfair Cipher encrypts messages using pairs of letters instead of single characters.",
    "To solve this puzzle, you will need to find the key and decrypt the message.",
    "Hmm... I wonder if the key is hidden near the sofa in front of the shelf.",
    "Or maybe it's somewhere around the computer next to the shelf.",
    "Also, the plaintext you need might be placed on the shelf.",
    "Take a look around carefully and use the Playfair Cipher to solve the puzzle.",
    "Alright...",
    "OH, You can use board at the right me to see the table",
    "Let’s begin the puzzle!"
};

    private int currentIndex = 0;
    private bool hasPlayed = false;

    private bool isTyping = false;
    private bool skipTyping = false;

    private bool timerStarted = false; // ⭐ สำคัญ
    private Coroutine activeCoroutine;

    void Start()
    {
        dialoguePanel.SetActive(false);

        if (closeButton != null)
            closeButton.SetActive(true);
    }

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
        }
    }

    public void ShowDoorLockedMessage()
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = "You can't leave this room yet.\nSolve the puzzle first!";
    }

    public void ShowMessage(string msg)
    {
        Debug.Log(msg);
    }

    void StartDialogueCoroutine(IEnumerator routine)
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(routine);
    }

    IEnumerator PlayDialogue()
    {
        while (currentIndex < dialogueLines.Length)
        {
            yield return StartCoroutine(TypeLine(dialogueLines[currentIndex]));

            // ⭐ ถ้าเป็นประโยคสุดท้าย
            if (currentIndex == dialogueLines.Length - 1)
            {
                StartTimerIfNeeded();
            }

            // รอคลิกเพื่อไปต่อ
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            currentIndex++;
        }

        CloseDialogue();
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
        Debug.Log("START TIMER");

        if (countdownTimer != null)
        {
            countdownTimer.gameObject.SetActive(true);
            countdownTimer.ShowTutorialTimer();
            countdownTimer.StartCountdownTutorial();
        }

        // 🧩 เริ่ม puzzle ตรงนี้เลย
        NetsecPuzzleManager.Instance.StartPuzzle();
    }

    // ================== CLOSE ==================

    // 🔘 เรียกจากปุ่ม Close Dialogue
    public void OnCloseButtonPressed()
    {
        StartTimerIfNeeded();
        CloseDialogue();
    }

    void CloseDialogue()
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }
}
