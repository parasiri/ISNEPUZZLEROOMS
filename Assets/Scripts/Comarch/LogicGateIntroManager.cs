using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogicGateIntroManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject introPanel;

    [Header("Description Text")]
    public TextMeshProUGUI descriptionText;

    [Header("Gate Cards")]
    public GameObject andCard;
    public GameObject orCard;
    public GameObject notCard;
    public GameObject nandCard;
    public GameObject norCard;
    public GameObject xorCard;
    public GameObject xnorCard;

    [Header("Continue Button")]
    public Button continueButton;

    [Header("Navigation Buttons")]
    public Button nextButton;
    public Button backButton;

    [Header("Description Lines")]
    public string[] descriptionLines =
    {
        "Logic gates are the basic building blocks of digital circuits.",
        "Each gate takes inputs (0 or 1) and produces one output.",
        "Different gates behave differently depending on their logic.",
        "Let's review the basic gates before you start the puzzle."
    };

    [Header("Ending Lines")]
    public string[] endingLines =
    {
        "Great! Now you understand the basic logic gates.",
        "You should be ready to solve the puzzles inside this room.",
        "Go ahead and show your skills!"
    };

    public NPCIntroDialogue npcIntro;

    private int index = 0;
    private int cardStep = 0;

    private enum IntroState
    {
        Description,
        Cards,
        Ending
    }

    private IntroState currentState = IntroState.Description;

    void Start()
    {
        introPanel.SetActive(true);

        continueButton.gameObject.SetActive(false);
        backButton.interactable = false;

        nextButton.onClick.AddListener(OnNext);
        backButton.onClick.AddListener(OnBack);

        HideAllCards();
        ShowDescription();
    }

    // =====================================================
    // NEXT BUTTON
    // =====================================================
    void OnNext()
    {
        if (currentState == IntroState.Description)
        {
            index++;

            if (index < descriptionLines.Length)
            {
                ShowDescription();
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

            if (cardStep < 7)
            {
                ShowCard();
            }
            else
            {
                HideAllCards();
                currentState = IntroState.Ending;
                index = 0;
                ShowEnding();
            }

            return;
        }

        if (currentState == IntroState.Ending)
        {
            index++;

            if (index < endingLines.Length)
            {
                ShowEnding();
            }
            else
            {
                nextButton.interactable = false;
                continueButton.gameObject.SetActive(true);
            }
        }
    }

    // =====================================================
    // BACK BUTTON
    // =====================================================
    void OnBack()
    {
        if (currentState == IntroState.Description)
        {
            if (index > 0)
            {
                index--;
                ShowDescription();
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
                currentState = IntroState.Description;
                index = descriptionLines.Length - 1;
                HideAllCards();
                ShowDescription();
            }

            return;
        }

        if (currentState == IntroState.Ending)
        {
            if (index > 0)
            {
                index--;
                ShowEnding();
            }
            else
            {
                currentState = IntroState.Cards;
                cardStep = 6;
                ShowCard();
            }
        }
    }

    // =====================================================
    // SHOW FUNCTIONS
    // =====================================================
    void ShowDescription()
    {
        descriptionText.text = descriptionLines[index];
    }

    void ShowEnding()
    {
        descriptionText.text = endingLines[index];
    }

    void ShowCard()
    {
        HideAllCards();
        descriptionText.text = "";

        if (cardStep == 0) andCard.SetActive(true);
        if (cardStep == 1) orCard.SetActive(true);
        if (cardStep == 2) notCard.SetActive(true);
        if (cardStep == 3) nandCard.SetActive(true);
        if (cardStep == 4) norCard.SetActive(true);
        if (cardStep == 5) xorCard.SetActive(true);
        if (cardStep == 6) xnorCard.SetActive(true);
    }

    void HideAllCards()
    {
        andCard.SetActive(false);
        orCard.SetActive(false);
        notCard.SetActive(false);
        nandCard.SetActive(false);
        norCard.SetActive(false);
        xorCard.SetActive(false);
        xnorCard.SetActive(false);
    }

    // =====================================================
    // CLOSE PANEL
    // =====================================================
    public void ClosePanel()
    {
        introPanel.SetActive(false);

        if (npcIntro != null)
            npcIntro.StartIntro();
    }
}