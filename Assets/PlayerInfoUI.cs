using UnityEngine;
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_Dropdown yearDropdown;
    public TMP_Dropdown careerDropdown;

    public GameObject popupPanel;

    public void SubmitPlayerInfo()
    {
        PlayerDataManager.Instance.playerName = nameInput.text;
        PlayerDataManager.Instance.playerYear = yearDropdown.options[yearDropdown.value].text;
        PlayerDataManager.Instance.playerCareer = careerDropdown.options[careerDropdown.value].text;

        Debug.Log("Player Name: " + PlayerDataManager.Instance.playerName);
        Debug.Log("Year: " + PlayerDataManager.Instance.playerYear);
        Debug.Log("Career: " + PlayerDataManager.Instance.playerCareer);

        popupPanel.SetActive(false);
    }
}