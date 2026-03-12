using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerDataManager;

public class ModeSelectManager : MonoBehaviour
{
    public GameObject roomSelectPanel;

    public void OpenRoomSelect()
    {
        roomSelectPanel.SetActive(true);
    }

    public void CloseRoomSelect()
    {
        roomSelectPanel.SetActive(false);
    }

    public void PlayAdventureMode()
    {
        PlayerDataManager.Instance.gameMode = GameMode.Adventure;

        SceneManager.LoadScene("IntroScene");
    }

    public void PlayRoom1()
    {
        PlayerDataManager.Instance.gameMode = GameMode.SingleRoom;
        PlayerDataManager.Instance.selectedRoom = 1;
        DoorTrigger.ResetRooms(); // กันค่าห้องเก่าค้าง

        SceneManager.LoadScene("Room_Algorithm");
    }

    public void PlayRoom2()
    {
        PlayerDataManager.Instance.gameMode = GameMode.SingleRoom;
        PlayerDataManager.Instance.selectedRoom = 2;
        DoorTrigger.ResetRooms(); // กันค่าห้องเก่าค้าง

        SceneManager.LoadScene("Room_Network");
    }

    public void PlayRoom3()
    {
        PlayerDataManager.Instance.gameMode = GameMode.SingleRoom;
        PlayerDataManager.Instance.selectedRoom = 3;
        DoorTrigger.ResetRooms(); // กันค่าห้องเก่าค้าง

        SceneManager.LoadScene("Room_NetworkSecurity");
    }

    public void PlayRoom4()
    {
        PlayerDataManager.Instance.gameMode = GameMode.SingleRoom;
        PlayerDataManager.Instance.selectedRoom = 4;
        DoorTrigger.ResetRooms(); // กันค่าห้องเก่าค้าง

        SceneManager.LoadScene("Room_OOP");
    }

    public void PlayRoom5()
    {
        PlayerDataManager.Instance.gameMode = GameMode.SingleRoom;
        PlayerDataManager.Instance.selectedRoom = 5;
        DoorTrigger.ResetRooms(); // กันค่าห้องเก่าค้าง

        SceneManager.LoadScene("Room_ComputerArchitecture");
    }
}