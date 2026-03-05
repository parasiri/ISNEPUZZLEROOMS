using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    // รายชื่อห้องทั้งหมด (ต้องตรงตามชื่อ Scene)
    public List<string> allRooms = new List<string>()
    {
        "Room_NetworkSecurity",
        "Room_ComputerArchitecture",
        "Room_Network",
        "Room_Algorithm",
        "Room_OOP"
    };

    private List<string> remainingRooms;

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

    private void Start()
    {
        ResetRooms(); // ตั้งค่ารอบใหม่
    }

    public void ResetRooms()
    {
        remainingRooms = new List<string>(allRooms);
    }

    //public void GoToNextRoom()
    //{
    //    if (remainingRooms == null || remainingRooms.Count == 0)
    //    {
    //        ResetRooms();
    //    }

    //    int index = Random.Range(0, remainingRooms.Count);
    //    string nextRoom = remainingRooms[index];

    //    remainingRooms.RemoveAt(index);

    //    SceneManager.LoadScene(nextRoom);
    //}

    public void GoToNextRoom()
    {
        if (remainingRooms == null || remainingRooms.Count == 0)
        {
            SceneManager.LoadScene("ScoreboardScene");
            return;
        }

        int index = Random.Range(0, remainingRooms.Count);
        string nextRoom = remainingRooms[index];
        remainingRooms.RemoveAt(index);
        SceneManager.LoadScene(nextRoom);
    }
}
