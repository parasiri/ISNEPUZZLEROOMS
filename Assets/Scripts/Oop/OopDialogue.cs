using UnityEngine;
using TMPro;
using System.Collections;

public class OOPDialogue : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Timer System")]
    public CountdownTimer countdownTimer;

    [Header("Optional")]
    public GameObject closeButton;

    [Header("Camera")]
    public Camera mainCamera;
    public Camera cameraRoom;


    private string[] dialogueLines =
    {
        "Welcome to the OOP Room.",
        "In this challenge, you will control a walker using code logic.",
        "Object-Oriented Programming is about controlling objects using instructions.",
        "Today, the walker is your object.",
        "To begin, click on the walker to start arranging movement commands.",
        "You can add Forward, Turn Left, or Turn Right.",
        "Plan your commands carefully before running them.",
        "When you are ready, press the RUN button to execute your code.",
        "If something goes wrong, press RESET to clear all commands.",
        "Your goal is to move the walker to the table at the back of the room.",
        "Reach the correct position in front of the table to complete the puzzle.",
        "Alright... let’s start coding!"
    };

    private int currentIndex = 0;
    private bool hasPlayed = false;

    private bool isTyping = false;
    private bool skipTyping = false;

    private bool timerStarted = false;
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

            if (currentIndex == dialogueLines.Length - 1)
            {
                StartTimerIfNeeded();
            }

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

    void StartTimerIfNeeded()
    {
        if (timerStarted) return;

        timerStarted = true;

        if (countdownTimer != null)
        {
            countdownTimer.gameObject.SetActive(true);
            countdownTimer.ShowTutorialTimer();
            countdownTimer.StartCountdownTutorial();
        }

        // เริ่ม OOP Puzzle
        OOPPuzzleManager.Instance.StartPuzzle();
    }

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

        SwitchToRoomCamera(); // 👈 เพิ่มบรรทัดนี้
    }
    void SwitchToRoomCamera()
    {
        if (mainCamera != null)
            mainCamera.gameObject.SetActive(false);

        if (cameraRoom != null)
            cameraRoom.gameObject.SetActive(true);
    }


}
