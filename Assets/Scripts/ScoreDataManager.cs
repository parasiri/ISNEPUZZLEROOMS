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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(PlayerScore newScore)
    {
        allScores.Add(newScore);

        // เรียงจากเวลาน้อยไปมาก และเก็บแค่ 10 อันดับ
        allScores = allScores
            .OrderBy(x => x.totalTime)
            .Take(10)
            .ToList();
    }

    public List<PlayerScore> GetTopScores(int topCount)
    {
        return allScores.Take(topCount).ToList();
    }
}