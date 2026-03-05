using UnityEngine;
using System.Collections.Generic;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    [Header("Player Info")]
    public string playerName;
    public string playerYear;
    public string playerCareer;

    [Header("Room Times")]
    public Dictionary<string, float> roomTimes = new Dictionary<string, float>();

    void Awake()
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

    public void SaveRoomTime(string roomName, float timeUsed)
    {
        if (!roomTimes.ContainsKey(roomName))
            roomTimes.Add(roomName, timeUsed);
    }

    public float GetTotalTime()
    {
        float total = 0f;

        foreach (float time in roomTimes.Values)
            total += time;

        return total;
    }
    public void ResetData()
    {
        roomTimes.Clear();
    }
}