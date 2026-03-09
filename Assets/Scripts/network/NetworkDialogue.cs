using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading;


public class NetworkDialogue : MonoBehaviour, IRoomDialogue
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    [Header("Timer System")]
    public CountdownTimer countdownTimer;
    [Header("Optional")]
    public GameObject closeButton; // ปุ่ม X หรือ Close
    public int lastdigit;
    public string ipaddress;
    public string[] sentences = { };
    private int currentIndex = 0;
    private bool hasPlayed = false;
    private bool isTyping = false;
    private bool skipTyping = false;
    private bool timerStarted = false;
    private Coroutine activeCoroutine;

    public DoorTrigger door;

    public computer1 computer1; // Reference to computer1 script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastdigit = Random.Range(1, 254);
        ipaddress = "192." + Random.Range(1, 254) + "." + Random.Range(1, 254) + "." + lastdigit.ToString();
        dialoguePanel.SetActive(false);

        sentences = new string[]
        {
            "Welcome to the Network room...",
            "In this room, you need to use your a given green PC to ping to server outside the room.",
            "Server ip address is " + ipaddress,
            "Subnet mask is /24",
            "you have to use command to ping server at the last steps.",
            "Be careful — you have to assign your PC ipaddress first.",
            "You can ask me anytime.",
            "Are you ready to setup your PC?"
        };

        if (closeButton != null)
            closeButton.SetActive(true);

        Debug.Log(ipaddress);
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

    // Update is called once per frame
    void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
                skipTyping = true;
        }
    }

    public void OpenPanel()
    {
        if (hasPlayed) return;

        hasPlayed = true;
        currentIndex = 0;

        dialoguePanel.SetActive(true);
        StartDialogueCoroutine(PlayDialogue());
    }

    void CloseDialogue()
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    public void OnCloseButtonPressed()
    {
        StartTimerIfNeeded();
        CloseDialogue();
    }

    IEnumerator PlayDialogue()
    {
        while (currentIndex < sentences.Length)
        {
            yield return StartCoroutine(TypeLine(sentences[currentIndex]));

            // ⭐ ถ้าเป็นประโยคสุดท้าย
            if (currentIndex == sentences.Length - 1)
            {
                StartTimerIfNeeded();
            }

            // รอคลิกเพื่อไปต่อ
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            currentIndex++;
        }
        computer1.open();
        CloseDialogue();
    }

    public void Outtro()
    {
        StartCoroutine(OuttroCoroutine());
    }

    public IEnumerator OuttroCoroutine()
    {
        countdownTimer.StopCountdown();
        dialoguePanel.SetActive(true);
        yield return TypeLine("Good job! Now, the door is unlocked. let's continue your journey!");
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        CloseDialogue();
        //door.Unlock();
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
        Debug.Log("START TIMER");

        if (countdownTimer != null)
        {
            countdownTimer.gameObject.SetActive(true);
            countdownTimer.ShowTutorialTimer();
            countdownTimer.StartCountdownTutorial();
        }

        // 🧩 เริ่ม puzzle ตรงนี้เลย


    }

    void StartDialogueCoroutine(IEnumerator routine)
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(routine);
    }


}