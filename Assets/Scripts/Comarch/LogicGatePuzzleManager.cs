using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class LogicGatePuzzleManager : MonoBehaviour
{
    public enum LogicGateType
    {
        AND, OR, NOT, NAND, NOR, XOR, XNOR
    }

    [System.Serializable]
    public class LogicGateData
    {
        public LogicGateType gateType;
        public Sprite gateSprite;
    }

    [Header("Logic Gates Data")]
    public LogicGateData[] logicGates;

    [Header("Frames")]
    public SpriteRenderer frame1Renderer;
    public SpriteRenderer frame2Renderer;

    [Header("Answer UI")]
    public GameObject answerPanel;
    public Button[] answerButtons;

    [Header("Dialogue System")]
    public ComacrhDialogue dialogue;

    public CountdownTimer countdownTimer;
    private bool isSaved = false;

    private LogicGateData frame1Gate;
    private LogicGateData frame2Gate;
    private LogicGateData selectedGate;
    private int selectedFrameIndex = -1;

    private bool frame1Solved = false;
    private bool frame2Solved = false;

    private bool puzzleStarted = false;

    // Phase control
    private bool phase1Completed = false;
    private bool phase2Started = false;
    private bool phase2Completed = false;

    private LogicGateData finalGate;

    [Header("Phase 2 UI")]
    public GameObject phase2Panel;
    public UnityEngine.UI.Image phase2GateImage;
    public TMPro.TextMeshProUGUI inputAText;
    public TMPro.TextMeshProUGUI inputBText;
    public TMPro.TextMeshProUGUI questionText;

    [Header("Phase 2 Buttons")]
    public Button phase2Button0;
    public Button phase2Button1;



    void Start()
    {
        answerPanel.SetActive(false);
        frame1Renderer.sprite = null;
        frame2Renderer.sprite = null;
        phase2Panel.SetActive(false);

    }

    // =========================
    // START PHASE 1
    // =========================
    public void StartPuzzle()
    {
        puzzleStarted = true;
        frame1Solved = false;
        frame2Solved = false;
        phase1Completed = false;
        phase2Started = false;
        phase2Completed = false;

        RandomizeFrames();
    }

    void RandomizeFrames()
    {
        List<LogicGateData> temp = new List<LogicGateData>(logicGates);

        frame1Gate = temp[Random.Range(0, temp.Count)];
        temp.Remove(frame1Gate);
        frame2Gate = temp[Random.Range(0, temp.Count)];

        frame1Renderer.sprite = frame1Gate.gateSprite;
        frame2Renderer.sprite = frame2Gate.gateSprite;
    }

    // =========================
    // SELECT FRAME (PHASE 1)
    // =========================
    public void SelectFrame(int frameIndex)
    {
        if (!puzzleStarted) return;

        if (frameIndex == 1 && frame1Solved) return;
        if (frameIndex == 2 && frame2Solved) return;

        selectedFrameIndex = frameIndex;
        selectedGate = (frameIndex == 1) ? frame1Gate : frame2Gate;

        ResetButtonColors();
        answerPanel.SetActive(true);
    }

    // =========================
    // ANSWER
    // =========================
    public void Answer(int answerIndex)
    {
        // ================= PHASE 2 =================
        if (phase2Started && !phase2Completed)
        {
            int correctOutput = CalculateOutput(finalGate.gateType, 1, 0);

            if (answerIndex == correctOutput)
            {
                phase2Completed = true;
                answerPanel.SetActive(false);

                if (dialogue != null)
                    dialogue.ShowFinalSuccess();
            }
            else
            {
                Debug.Log("Wrong Output!");
            }

            return;
        }

        // ================= PHASE 1 =================
        if (!puzzleStarted || selectedGate == null) return;

        LogicGateType selectedAnswer = (LogicGateType)answerIndex;

        Button clickedButton =
            UnityEngine.EventSystems.EventSystem.current
            .currentSelectedGameObject
            .GetComponent<Button>();

        if (selectedAnswer == selectedGate.gateType)
        {
            clickedButton.image.color = Color.green;

            if (selectedFrameIndex == 1) frame1Solved = true;
            if (selectedFrameIndex == 2) frame2Solved = true;

            answerPanel.SetActive(false);

            if (dialogue != null)
                dialogue.ShowCorrectFeedback();

            if (frame1Solved && frame2Solved)
            {
                puzzleStarted = false;
                phase1Completed = true;

                if (dialogue != null)
                    dialogue.ShowPhase2Intro();
            }
        }
        else
        {
            clickedButton.image.color = Color.red;
        }
    }

    void ResetButtonColors()
    {
        foreach (Button btn in answerButtons)
            btn.image.color = Color.white;
    }

    // =========================
    // START PHASE 2
    // =========================
    public void StartPhase2()
    {
        if (!phase1Completed || phase2Started) return;

        phase2Started = true;

        List<LogicGateData> temp = new List<LogicGateData>(logicGates);
        finalGate = temp[Random.Range(0, temp.Count)];

        // เปิด popup
        phase2Panel.SetActive(true);

        // ใส่รูป gate
        phase2GateImage.sprite = finalGate.gateSprite;

        // Fix input
        inputAText.text = "Input A = 1";
        inputBText.text = "Input B = 0";
        questionText.text = "What is the output?";

        phase2Button0.image.color = Color.white;
        phase2Button1.image.color = Color.white;

    }

    public void AnswerPhase2(int answerValue)
    {
        if (!phase2Started || phase2Completed) return;

        int correctOutput = CalculateOutput(finalGate.gateType, 1, 0);

        Button clickedButton =
            UnityEngine.EventSystems.EventSystem.current
            .currentSelectedGameObject
            .GetComponent<Button>();

        if (answerValue == correctOutput)
        {
            phase2Completed = true;

            clickedButton.image.color = Color.green;

            // ===== STOP TIME =====
            if (!isSaved)
            {
                isSaved = true;

                countdownTimer.StopCountdown();

                float usedTime = countdownTimer.GetTimeUsed();

                if (PlayerDataManager.Instance != null)
                {
                    PlayerDataManager.Instance.SaveRoomTime(
                        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                        usedTime
                    );
                }
            }

            StartCoroutine(ClosePhase2AfterDelay());
        }
        else
        {
            clickedButton.image.color = Color.red;
        }
    }

    IEnumerator ClosePhase2AfterDelay()
    {
        yield return new WaitForSeconds(0.8f);

        phase2Panel.SetActive(false);

        if (dialogue != null)
            dialogue.ShowFinalSuccess();
    }





    // =========================
    // CALCULATE OUTPUT
    // =========================
    int CalculateOutput(LogicGateType gate, int inputA, int inputB)
    {
        switch (gate)
        {
            case LogicGateType.AND: return inputA & inputB;
            case LogicGateType.OR: return inputA | inputB;
            case LogicGateType.NOT: return inputA == 0 ? 1 : 0;
            case LogicGateType.NAND: return (inputA & inputB) == 1 ? 0 : 1;
            case LogicGateType.NOR: return (inputA | inputB) == 1 ? 0 : 1;
            case LogicGateType.XOR: return inputA ^ inputB;
            case LogicGateType.XNOR: return (inputA ^ inputB) == 1 ? 0 : 1;
        }

        return 0;
    }
}
