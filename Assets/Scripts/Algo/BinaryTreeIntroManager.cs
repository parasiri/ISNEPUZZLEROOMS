using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BinaryTreeIntroManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject introPanel;

    [Header("Description Text")]
    public TextMeshProUGUI descriptionText;

    [Header("Tree Cards")]
    public GameObject binaryTreeCard;
    public GameObject bstCard;
    public GameObject balancedTreeCard;

    [Header("Continue Button")]
    public Button continueButton;

    [Header("Description Lines")]
    public string[] descriptionLines =
    {
        "In this room, you will learn about Binary Trees.",
        "Binary Trees are used in searching, sorting, and many algorithms.",
        "Let's explore three important types of tree structures.",
        "Tap to continue and review each tree type."
    };

    [Header("Ending Lines")]
    public string[] endingLines =
    {
        "Great! Now you understand the basics of tree data structures.",
        "This knowledge will help you solve the upcoming puzzles.",
        "Good luck and have fun!"
    };

    public NPCIntroDialogue npcIntro;

    private int index = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    private int cardStep = 0;
    private bool endingStarted = false;

    void Start()
    {
        introPanel.SetActive(true);

        binaryTreeCard.SetActive(false);
        bstCard.SetActive(false);
        balancedTreeCard.SetActive(false);

        continueButton.gameObject.SetActive(false);

        StartCoroutine(PlayDescription());
    }

    void Update()
    {
        if (!introPanel.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                skipTyping = true;
                return;
            }

            if (index >= descriptionLines.Length && !endingStarted)
            {
                ShowNextCard();
                return;
            }

            index++;
        }
    }

    IEnumerator PlayDescription()
    {
        while (index < descriptionLines.Length)
        {
            yield return StartCoroutine(TypeLine(descriptionLines[index]));

            float t = 0f;
            float waitTime = 0.5f;

            while (t < waitTime)
            {
                if (Input.GetMouseButtonDown(0)) break;
                t += Time.deltaTime;
                yield return null;
            }

            index++;
        }

        ShowNextCard();
    }

    IEnumerator TypeLine(string line)
    {
        descriptionText.text = "";
        isTyping = true;
        skipTyping = false;

        foreach (char c in line)
        {
            if (skipTyping)
            {
                descriptionText.text = line;
                break;
            }

            descriptionText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    void ShowNextCard()
    {
        descriptionText.text = "";
        cardStep++;

        binaryTreeCard.SetActive(false);
        bstCard.SetActive(false);
        balancedTreeCard.SetActive(false);

        if (cardStep == 1)
        {
            binaryTreeCard.SetActive(true);
            return;
        }

        if (cardStep == 2)
        {
            bstCard.SetActive(true);
            return;
        }

        if (cardStep == 3)
        {
            balancedTreeCard.SetActive(true);
            return;
        }

        if (!endingStarted)
        {
            endingStarted = true;
            StartCoroutine(PlayEndingDescription());
        }
    }

    IEnumerator PlayEndingDescription()
    {
        descriptionText.text = "";

        foreach (string line in endingLines)
        {
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(0.4f);

            while (!Input.GetMouseButtonDown(0))
                yield return null;
        }

        continueButton.gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        introPanel.SetActive(false);

        // 🔥 เรียกให้ NPC เล่น intro ต่อ
        if (npcIntro != null)
            npcIntro.StartIntro();
    }
}
