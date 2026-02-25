using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class OOPIntroManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject introPanel;

    [Header("Description Text")]
    public TextMeshProUGUI descriptionText;

    [Header("Concept Cards")]
    public GameObject classCard;
    public GameObject objectCard;
    public GameObject methodCard;
    public GameObject commandCard;

    [Header("Navigation Buttons")]
    public Button nextButton;
    public Button backButton;

    [Header("Continue Button")]
    public Button continueButton;

    [Header("Intro Lines")]
    public string[] introLines =
    {
        "Welcome to the OOP Room.",
        "In this room, you will learn about Object-Oriented Programming.",
        "OOP helps us organize code into reusable structures.",
        "Let’s explore the core concepts step by step."
    };

    [Header("Ending Lines")]
    public string[] endingLines =
    {
        "In this puzzle, each command is an object.",
        "Each object has its own behavior or method.",
        "Use the correct objects to guide the walker to the goal.",
        "Good luck!"
    };

    

    private int index = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    private int cardStep = 0;
    private bool endingStarted = false;

    void Start()
    {
        introPanel.SetActive(true);

        classCard.SetActive(false);
        objectCard.SetActive(false);
        methodCard.SetActive(false);
        commandCard.SetActive(false);

        continueButton.gameObject.SetActive(false);

        backButton.interactable = false;

        nextButton.onClick.AddListener(OnNext);
        backButton.onClick.AddListener(OnBack);

        ShowIntroLine();
    }
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

            if (cardStep < 4)
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
                cardStep = 3;
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
        classCard.SetActive(false);
        objectCard.SetActive(false);
        methodCard.SetActive(false);
        commandCard.SetActive(false);

        if (cardStep == 0) classCard.SetActive(true);
        if (cardStep == 1) objectCard.SetActive(true);
        if (cardStep == 2) methodCard.SetActive(true);
        if (cardStep == 3) commandCard.SetActive(true);
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

    //        // เพิ่มตรงนี้
    //        if (index >= introLines.Length && !endingStarted)
    //        {
    //            ShowNextCard();
    //            return;
    //        }

    //        index++;
    //    }
    //}

    private enum IntroState
    {
        IntroText,
        Cards,
        Ending
    }

    private IntroState currentState = IntroState.IntroText;

    IEnumerator PlayIntro()
    {
        while (index < introLines.Length)
        {
            yield return StartCoroutine(TypeLine(introLines[index]));
            yield return WaitForClick();
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

    IEnumerator WaitForClick()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;
    }

    void ShowNextCard()
    {
        descriptionText.text = "";

        cardStep++;

        classCard.SetActive(false);
        objectCard.SetActive(false);
        methodCard.SetActive(false);
        commandCard.SetActive(false);

        if (cardStep == 1)
        {
            classCard.SetActive(true);
            //descriptionText.text = "Class is a blueprint for creating objects.";
            return;
        }

        if (cardStep == 2)
        {
            objectCard.SetActive(true);
            //descriptionText.text = "Object is an instance created from a class.";
            return;
        }

        if (cardStep == 3)
        {
            methodCard.SetActive(true);
            //descriptionText.text = "Methods define the behavior of an object.";
            return;
        }

        if (cardStep == 4)
        {
            commandCard.SetActive(true);
            //descriptionText.text = "In this puzzle, each command is an object with its own behavior.";
            return;
        }

        if (!endingStarted)
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
    void HideAllCards()
    {
        classCard.SetActive(false);
        objectCard.SetActive(false);
        methodCard.SetActive(false);
        commandCard.SetActive(false);
    }

    public void ClosePanel()
    {
        introPanel.SetActive(false);

        // เริ่ม Puzzle ทันทีหลังจบ Intro
        if (OOPPuzzleManager.Instance != null)
            OOPPuzzleManager.Instance.StartPuzzle();
    }
}