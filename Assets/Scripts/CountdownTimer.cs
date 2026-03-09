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

    private float elapsedTime = 0f;
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
        return elapsedTime;
    }

    // 🔹 ใช้กับทุกห้องได้ 
    public void ShowTutorialTimer()
    {
        timerText.gameObject.SetActive(true);

        if (hintButton != null)
            hintButton.gameObject.SetActive(true);

        elapsedTime = 0f;
        UpdateTimerDisplay();
    }

    public void StartCountdownTutorial()
    {
        elapsedTime = 0f;      // ⭐ รีเซ็ตเวลา
        StopCountdown();       // ⭐ หยุด coroutine เก่าถ้ามี

        if (!isCounting)
            timerCoroutine = StartCoroutine(TutorialCountdown());
    }

    IEnumerator TutorialCountdown()
    {
        isCounting = true;

        while (isCounting)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
        }

        timerCoroutine = null;
    }


    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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

    

}
