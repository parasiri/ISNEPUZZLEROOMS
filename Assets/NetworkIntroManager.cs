using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkIntroManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject introPanel;

    [Header("Description Text")]
    public TextMeshProUGUI descriptionText;

    [Header("Concept Cards")]
    public GameObject ipCard;
    public GameObject subnetCard;
    public GameObject networkCard;
    public GameObject puzzleCard;

    [Header("Navigation Buttons")]
    public Button nextButton;
    public Button backButton;

    [Header("Continue Button")]
    public Button continueButton;

    [Header("Intro Lines")]
    public string[] introLines =
    {
        "Welcome to the Network Room.",
        "In this room, you will learn how devices communicate in a network.",
        "Every device needs an IP Address to identify itself.",
        "To understand networks better, we must also understand Subnet Masks."
    };

    [Header("Ending Lines")]
    public string[] endingLines =
    {
        "An IP Address identifies a device in the network.",
        "A Subnet Mask tells us which part is the network and which part is the host.",
        "For example, /24 means the first 24 bits belong to the network.",
        "Use this knowledge to solve the network puzzle."
    };

    private int index = 0;
    private int cardStep = 0;

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

        ipCard.SetActive(false);
        subnetCard.SetActive(false);
        networkCard.SetActive(false);
        puzzleCard.SetActive(false);

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
        ipCard.SetActive(false);
        subnetCard.SetActive(false);
        networkCard.SetActive(false);
        puzzleCard.SetActive(false);

        if (cardStep == 0) ipCard.SetActive(true);
        if (cardStep == 1) subnetCard.SetActive(true);
        if (cardStep == 2) networkCard.SetActive(true);
        if (cardStep == 3) puzzleCard.SetActive(true);
    }

    void HideAllCards()
    {
        ipCard.SetActive(false);
        subnetCard.SetActive(false);
        networkCard.SetActive(false);
        puzzleCard.SetActive(false);
    }

    public void ClosePanel()
    {
        introPanel.SetActive(false);

    }
}