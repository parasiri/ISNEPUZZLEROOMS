using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public string playerYear;
    public string playerCareer;
    public float totalTime;

    public PlayerScore(string name, string year, string career, float time)
    {
        playerName = name;
        playerYear = year;
        playerCareer = career;
        totalTime = time;
    }
}

[System.Serializable]
public class ScoreList
{
    public List<PlayerScore> scores = new List<PlayerScore>();
}

public class ScoreDataManager : MonoBehaviour
{
    public static ScoreDataManager Instance;

    public List<PlayerScore> allScores = new List<PlayerScore>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(PlayerScore newScore)
    {
        allScores.Add(newScore);

        allScores = allScores
            .OrderBy(x => x.totalTime)
            .Take(10)
            .ToList();

        SaveScores();
    }

    public List<PlayerScore> GetTopScores(int topCount)
    {
        return allScores.Take(topCount).ToList();
    }

    void SaveScores()
    {
        ScoreList list = new ScoreList();
        list.scores = allScores;

        string json = JsonUtility.ToJson(list);

        PlayerPrefs.SetString("Leaderboard", json);
        PlayerPrefs.Save();
    }

    void LoadScores()
    {
        if (PlayerPrefs.HasKey("Leaderboard"))
        {
            string json = PlayerPrefs.GetString("Leaderboard");

            ScoreList list = JsonUtility.FromJson<ScoreList>(json);

            allScores = list.scores;
        }
    }
}