using UnityEngine;
using TMPro;

public class PlayfairBoardUI : MonoBehaviour
{
    public GameObject boardPanel;
    public TextMeshProUGUI[] gridTexts; // 25 ช่อง

    public TextMeshProUGUI plaintextText; 

    public void OpenBoard()
    {
        string key = NetsecPuzzleManager.Instance.currentKey;
        string plaintext = NetsecPuzzleManager.Instance.currentPlaintext; // ดึง plaintext

        char[,] table = PlayfairCipher.GeneratePlayfairTable(key);

        int index = 0;

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                gridTexts[index].text = table[row, col].ToString();
                index++;
            }
        }

        // แสดง plaintext
        plaintextText.text = "Plaintext: " + plaintext;

        boardPanel.SetActive(true);
    }

    public void CloseBoard()
    {
        boardPanel.SetActive(false);
    }
}