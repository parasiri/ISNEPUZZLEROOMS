using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DoorTrigger : MonoBehaviour
{
    // รายชื่อห้องที่ต้องเล่น
    public static List<string> remainingRooms = new List<string>()
    {
        "Room_NetworkSecurity",
        "Room_ComputerArchitecture",
        "Room_Network",
        "Room_Algorithm",
        "Room_OOP"
    };

    public string scoreboardScene = "Scoreboard";

    public MonoBehaviour roomDialogue;

    private void OnMouseDown()
    {
        // แก้ได้ค่อยเปิดประตู
        if (PuzzleStateManager.Instance != null &&
        !PuzzleStateManager.Instance.IsPuzzleSolved())
        {
            Debug.Log("Solve the puzzle first!");

            IRoomDialogue dialogue = roomDialogue as IRoomDialogue;

            if (dialogue != null)
            {
                dialogue.ShowDoorLockedMessage();
            }

            return;
        }
        PuzzleStateManager.Instance.ResetPuzzle();

        // ห้องเดียว
        if (PlayerDataManager.Instance.gameMode == PlayerDataManager.GameMode.SingleRoom)
        {
            SceneManager.LoadScene(scoreboardScene);
            return;
        }

        // ถ้าเล่นครบทุกห้องแล้ว → ไป Scoreboard
        if (remainingRooms.Count == 0)
        {
            SceneManager.LoadScene(scoreboardScene);
            return;
        }

        // สุ่มห้องจากที่เหลือ
        int index = Random.Range(0, remainingRooms.Count);
        string selectedRoom = remainingRooms[index];

        // ลบออกเพื่อกันซ้ำ
        remainingRooms.RemoveAt(index);

        // โหลดห้อง
        SceneManager.LoadScene(selectedRoom);
    }
    public static void ResetRooms()
    {
        remainingRooms = new List<string>()
    {
        "Room_NetworkSecurity",
        "Room_ComputerArchitecture",
        "Room_Network",
        "Room_Algorithm",
        "Room_OOP"
    };
    }
}
