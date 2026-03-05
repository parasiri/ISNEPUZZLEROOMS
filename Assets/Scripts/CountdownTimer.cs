using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public static CountdownTimer Instance;

    [Header("Timer")]
    public TextMeshProUGUI timerText;

    [Header("Hint System")]
    public Button hintButton;
    public GameObject hintTextPanel;
    public Button closeHintButton;

    private float timeLeft = 60f;
    private bool isCounting = false;

    private Coroutine timerCoroutine;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //timerText.gameObject.SetActive(false);

        if (hintButton != null)
            hintButton.gameObject.SetActive(false);

        if (hintTextPanel != null)
            hintTextPanel.SetActive(false);

        if (hintButton != null)
            hintButton.onClick.AddListener(ShowHint);

        if (closeHintButton != null)
            closeHintButton.onClick.AddListener(CloseHint);
    }


    
    //เก็บเวลาในแต่ละห้อง
    public float GetTimeUsed()
    {
        return 300f - timeLeft;
    }

    // 🔹 ใช้กับทุกห้องได้ 
    public void ShowTutorialTimer()
    {
        timerText.gameObject.SetActive(true);

        if (hintButton != null)
            hintButton.gameObject.SetActive(true);

        timeLeft = 300f;
        UpdateTimerDisplay();
    }

    public void StartCountdownTutorial()
{
    Debug.Log("StartCountdownTutorial called, isCounting = " + isCounting);

    if (!isCounting)
        timerCoroutine = StartCoroutine(TutorialCountdown());
}


    IEnumerator TutorialCountdown()
    {
        isCounting = true;

        while (timeLeft > 0 && isCounting)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
        }

        if (timeLeft <= 0)
        {
            timerText.text = "Time’s up!";
        }

        isCounting = false;
        timerCoroutine = null;
    }


    void UpdateTimerDisplay()
    {
        timerText.text = timeLeft.ToString("0.0");
    }

    public void StopCountdown()
    {
        isCounting = false;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }


    // ---- Hint System ----
    public void ShowHint()
    {
        if (hintTextPanel != null)
            hintTextPanel.SetActive(true);
    }

    public void CloseHint()
    {
        if (hintTextPanel != null)
            hintTextPanel.SetActive(false);
    }

    public void ReduceTime(float seconds)
    {
        timeLeft -= seconds;
        if (timeLeft < 0)
            timeLeft = 0;

        UpdateTimerDisplay();
    }

}
