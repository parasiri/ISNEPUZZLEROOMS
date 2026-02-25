using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayfairIntroManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject introPanel;

    [Header("Description Text")]
    public TextMeshProUGUI descriptionText;

    [Header("Cards")]
    public GameObject keySquareCard;
    public GameObject ruleCard;
    public GameObject ruletwoCard;
    public GameObject encryptCard;
    public GameObject decryptCard;

    [Header("Continue Button")]
    public Button continueButton;

    [Header("Navigation Buttons")]
    public Button nextButton;
    public Button backButton;

    [Header("Intro Lines")]
    public string[] introLines =
    {
        "Welcome to the Network Security Room!",
        "Before you begin the puzzle, let's learn the basics of the Playfair Cipher.",
        "Playfair Cipher encrypts text by splitting it into letter pairs.",
        "It uses a 5x5 key square generated from a keyword.",
        "Let's explore how Playfair works step by step."
    };

    [Header("Ending Lines")]
    public string[] endingLines =
    {
        "Great! Now you understand the structure of the Playfair Cipher.",
        "Use this knowledge to solve the encryption puzzle.",
        "Good luck, agent."
    };

    public NPCIntroDialogue npcIntro;

    private int index = 0;
    private int cardStep = 0;
    private bool isTyping = false;
    private bool skipTyping = false;
    private bool endingStarted = false;
    private enum IntroState
    {
        IntroText,
        Cards,
        Ending
    }

    private IntroState currentState = IntroState.IntroText;

    void Start()
    {
        introPanel.SetActive(true);

        keySquareCard.SetActive(false);
        ruleCard.SetActive(false);
        ruletwoCard.SetActive(false);
        encryptCard.SetActive(false);
        decryptCard.SetActive(false);

        continueButton.gameObject.SetActive(false);

        backButton.interactable = false;

        nextButton.onClick.AddListener(OnNext);
        backButton.onClick.AddListener(OnBack);

        ShowIntroLine();
    }

    //void Update()
    //{
    //    if (!introPanel.activeSelf) return;

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if (isTyping)
    //        {
    //            skipTyping = true;
    //            return;
    //        }

    //        if (index >= introLines.Length && !endingStarted)
    //        {
    //            ShowNextCard();
    //            return;
    //        }

    //        index++;
    //    }
    //}

    //IEnumerator PlayIntro()
    //{
    //    while (index < introLines.Length)
    //    {
    //        yield return StartCoroutine(TypeLine(introLines[index]));
    //        yield return WaitForClick();
    //        index++;
    //    }

    //    ShowNextCard();
    //}
    void OnNext()
    {
        if (currentState == IntroState.IntroText)
        {
            index++;

            if (index < introLines.Length)
            {
                ShowIntroLine();
            }
            else
            {
                currentState = IntroState.Cards;
                cardStep = 0;
                ShowCard();
            }

            backButton.interactable = true;
            return;
        }

        if (currentState == IntroState.Cards)
        {
            cardStep++;

            if (cardStep < 5)
            {
                ShowCard();
            }
            else
            {
                HideAllCards();
                currentState = IntroState.Ending;
                index = 0;
                ShowEndingLine();
            }
            return;
        }

        if (currentState == IntroState.Ending)
        {
            index++;

            if (index < endingLines.Length)
            {
                ShowEndingLine();
            }
            else
            {
                nextButton.interactable = false;
                continueButton.gameObject.SetActive(true);
            }
        }
    }
    void OnBack()
    {
        if (currentState == IntroState.IntroText)
        {
            if (index > 0)
            {
                index--;
                ShowIntroLine();
            }

            if (index == 0)
                backButton.interactable = false;

            return;
        }

        if (currentState == IntroState.Cards)
        {
            if (cardStep > 0)
            {
                cardStep--;
                ShowCard();
            }
            else
            {
                HideAllCards();
                currentState = IntroState.IntroText;
                index = introLines.Length - 1;
                ShowIntroLine();
            }
            return;
        }

        if (currentState == IntroState.Ending)
        {
            if (index > 0)
            {
                index--;
                ShowEndingLine();
            }
            else
            {
                currentState = IntroState.Cards;
                cardStep = 4;
                ShowCard();
            }
        }
    }
    void ShowIntroLine()
    {
        descriptionText.text = introLines[index];
    }

    void ShowEndingLine()
    {
        descriptionText.text = endingLines[index];
    }

    void ShowCard()
    {
        keySquareCard.SetActive(false);
        ruleCard.SetActive(false);
        ruletwoCard.SetActive(false);
        encryptCard.SetActive(false);
        decryptCard.SetActive(false);

        if (cardStep == 0) keySquareCard.SetActive(true);
        if (cardStep == 1) ruleCard.SetActive(true);
        if (cardStep == 2) ruletwoCard.SetActive(true);
        if (cardStep == 3) encryptCard.SetActive(true);
        if (cardStep == 4) decryptCard.SetActive(true);
    }

    void HideAllCards()
    {
        keySquareCard.SetActive(false);
        ruleCard.SetActive(false);
        ruletwoCard.SetActive(false);
        encryptCard.SetActive(false);
        decryptCard.SetActive(false);
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

    IEnumerator WaitForClick()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;
    }

    void ShowNextCard()
    {
        descriptionText.text = "";
        cardStep++;

        keySquareCard.SetActive(false);
        ruleCard.SetActive(false);
        ruletwoCard.SetActive(false);
        encryptCard.SetActive(false);
        decryptCard.SetActive(false);

        if (cardStep == 1) keySquareCard.SetActive(true);
        else if (cardStep == 2) ruleCard.SetActive(true);
        else if (cardStep == 3) ruletwoCard.SetActive(true);
        else if (cardStep == 4) encryptCard.SetActive(true);
        else if (cardStep == 5) decryptCard.SetActive(true);
        else if (!endingStarted)
        {
            endingStarted = true;
            StartCoroutine(PlayEnding());
        }
    }

    IEnumerator PlayEnding()
    {
        descriptionText.text = "";

        foreach (string line in endingLines)
        {
            yield return StartCoroutine(TypeLine(line));
            yield return WaitForClick();
        }

        continueButton.gameObject.SetActive(true);
    }

    // 🔥 ปุ่ม Continue เรียกฟังก์ชันนี้
    public void ClosePanel()
    {
        introPanel.SetActive(false);

        // 🔥 เรียกให้ NPC เล่น intro ต่อ
        if (npcIntro != null)
            npcIntro.StartIntro();
    }
}
