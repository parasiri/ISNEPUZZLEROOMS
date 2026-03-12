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
    //public List<PlayerScore> scores = new List<PlayerScore>();
    public List<PlayerScore> adventureScores = new List<PlayerScore>();

    public List<PlayerScore> room1Scores = new List<PlayerScore>();
    public List<PlayerScore> room2Scores = new List<PlayerScore>();
    public List<PlayerScore> room3Scores = new List<PlayerScore>();
    public List<PlayerScore> room4Scores = new List<PlayerScore>();
    public List<PlayerScore> room5Scores = new List<PlayerScore>();

}

public class ScoreDataManager : MonoBehaviour
{
    public static ScoreDataManager Instance;

    //public List<PlayerScore> allScores = new List<PlayerScore>();
    public List<PlayerScore> adventureScores = new List<PlayerScore>();

    public List<PlayerScore> room1Scores = new List<PlayerScore>();
    public List<PlayerScore> room2Scores = new List<PlayerScore>();
    public List<PlayerScore> room3Scores = new List<PlayerScore>();
    public List<PlayerScore> room4Scores = new List<PlayerScore>();
    public List<PlayerScore> room5Scores = new List<PlayerScore>();

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

    //public void AddScore(PlayerScore newScore)
    //{
    //    allScores.Add(newScore);

    //    allScores = allScores
    //        .OrderBy(x => x.totalTime)
    //        .Take(10)
    //        .ToList();

    //    SaveScores();
    //}
    public void AddScore(PlayerScore newScore)
    {
        if (PlayerDataManager.Instance.gameMode == PlayerDataManager.GameMode.Adventure)
        {
            AddToList(adventureScores, newScore);
        }
        else
        {
            int room = PlayerDataManager.Instance.selectedRoom;

            if (room == 1) AddToList(room1Scores, newScore);
            if (room == 2) AddToList(room2Scores, newScore);
            if (room == 3) AddToList(room3Scores, newScore);
            if (room == 4) AddToList(room4Scores, newScore);
            if (room == 5) AddToList(room5Scores, newScore);
        }

        SaveScores();
    }

    void AddToList(List<PlayerScore> list, PlayerScore score)
    {
        list.Add(score);

        list.Sort((a, b) => a.totalTime.CompareTo(b.totalTime));

        if (list.Count > 10)
            list.RemoveRange(10, list.Count - 10);
    }

    public List<PlayerScore> GetTopScores(int topCount)
    {
        if (PlayerDataManager.Instance.gameMode == PlayerDataManager.GameMode.Adventure)
            return adventureScores.Take(topCount).ToList();

        int room = PlayerDataManager.Instance.selectedRoom;

        if (room == 1) return room1Scores.Take(topCount).ToList();
        if (room == 2) return room2Scores.Take(topCount).ToList();
        if (room == 3) return room3Scores.Take(topCount).ToList();
        if (room == 4) return room4Scores.Take(topCount).ToList();
        if (room == 5) return room5Scores.Take(topCount).ToList();

        return new List<PlayerScore>();
    }

    void SaveScores()
    {
        ScoreList list = new ScoreList();
        list.adventureScores = adventureScores;

        list.room1Scores = room1Scores;
        list.room2Scores = room2Scores;
        list.room3Scores = room3Scores;
        list.room4Scores = room4Scores;
        list.room5Scores = room5Scores;

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

            adventureScores = list.adventureScores;

            room1Scores = list.room1Scores;
            room2Scores = list.room2Scores;
            room3Scores = list.room3Scores;
            room4Scores = list.room4Scores;
            room5Scores = list.room5Scores;
        }
    }
}