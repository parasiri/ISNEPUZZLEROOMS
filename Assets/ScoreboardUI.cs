using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreboardUI : MonoBehaviour
{
    public TMP_Text playerInfoText;
    public TMP_Text timeText;
    public TMP_Text rankingText;

    public Transform rankingContent;
    public GameObject rankingRowPrefab;

    public TMP_Text nameText;
    public TMP_Text yearText;
    public TMP_Text careerText;

    bool scoreAdded = false;

    void ShowRanking()
    {
        var topPlayers = ScoreDataManager.Instance.GetTopScores(10);

        if (PlayerDataManager.Instance.gameMode == PlayerDataManager.GameMode.Adventure)
        {
            rankingText.text = "ADVENTURE LEADERBOARD\n\n";
        }
        else
        {
            rankingText.text = "ROOM LEADERBOARD\n\n";
        }

        foreach (Transform child in rankingContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < topPlayers.Count; i++)
        {
            GameObject row = Instantiate(rankingRowPrefab, rankingContent);

            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            string medal = "";
            if (i == 0) medal = "GOD";   
            else if (i == 1) medal = "GOOD"; 
            else if (i == 2) medal = "D"; 

            texts[0].text = medal + " " + (i + 1);
            texts[1].text = topPlayers[i].playerName;
            texts[2].text = topPlayers[i].totalTime.ToString("0.0") + " sec";
        }
    }

    void Start()
    {
        if (PlayerDataManager.Instance == null) return;

        var data = PlayerDataManager.Instance;

        float totalTime;

        if (data.gameMode == PlayerDataManager.GameMode.Adventure)
        {
            totalTime = data.GetTotalTime();
        }
        else
        {
            // ถ้าเล่นห้องเดียว ให้ใช้เวลาเฉพาะห้องนั้น
            totalTime = data.GetTotalTime();
        }

        nameText.text = data.playerName;
        yearText.text = data.playerYear;
        careerText.text = data.playerCareer;
        timeText.text = totalTime.ToString("0.0") + " sec";

        if (!scoreAdded && ScoreDataManager.Instance != null)
        {
            PlayerScore newScore = new PlayerScore(
                data.playerName,
                data.playerYear,
                data.playerCareer,
                totalTime
            );

            ScoreDataManager.Instance.AddScore(newScore);
            scoreAdded = true;
        }
        ShowRanking();
    }

    // ⭐ ปุ่ม Play Again
    public void PlayAgain()
    {
        PlayerDataManager.Instance.ResetData();
        DoorTrigger.ResetRooms();
        SceneManager.LoadScene("MainMenu");
    }

    // ⭐ ปุ่ม Main Menu
    public void BackToMainMenu()
    {
        PlayerDataManager.Instance.ResetData();
        DoorTrigger.ResetRooms();
        SceneManager.LoadScene("MainMenu");
    }
}