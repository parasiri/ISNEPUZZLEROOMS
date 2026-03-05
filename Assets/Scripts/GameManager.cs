using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_InputField NameInput;
    public TMP_Dropdown YearDropdown;
    public TMP_Dropdown CareerDropdown;

    public GameObject popupPanel;
    public GameObject confirmPanel;
    public TMP_Text summaryText;

    // ฟังก์ชันสำหรับปุ่ม Start หน้าแรก
    public void StartGame()
    {
        popupPanel.SetActive(true);
    }

    // ฟังก์ชันสำหรับปุ่ม Next
    public void GoToConfirmPage()
    {
        string playerName = NameInput.text;
        string playerYear = YearDropdown.options[YearDropdown.value].text;
        string playerCareer = CareerDropdown.options[CareerDropdown.value].text;

        summaryText.text =
            "Your name is: " + playerName + "\n" +
            "Your year is: " + playerYear + "\n" +
            "Your interested role: " + playerCareer;

        confirmPanel.SetActive(true);
        popupPanel.SetActive(false);
    }

    // ฟังก์ชันปุ่ม Back
    public void BackToInput()
    {
        confirmPanel.SetActive(false);
        popupPanel.SetActive(true);
    }

    // ฟังก์ชันปุ่ม Confirm
    public void ConfirmAndStartGame()
    {
        PlayerDataManager.Instance.playerName = NameInput.text;
        PlayerDataManager.Instance.playerYear =
            YearDropdown.options[YearDropdown.value].text;
        PlayerDataManager.Instance.playerCareer =
            CareerDropdown.options[CareerDropdown.value].text;

        SceneManager.LoadScene("IntroScene");
    }
    //public void ConfirmAndStartGame()
    //{
    //    PlayerDataManager.Instance.playerName = NameInput.text;
    //    PlayerDataManager.Instance.playerYear =
    //        YearDropdown.options[YearDropdown.value].text;
    //    PlayerDataManager.Instance.playerCareer =
    //        CareerDropdown.options[CareerDropdown.value].text;

    //    SceneManager.LoadScene("IntroScene");
    //}
}
