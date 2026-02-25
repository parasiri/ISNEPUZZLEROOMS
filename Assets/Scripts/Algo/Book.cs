using UnityEngine;
using TMPro;

public class Book : MonoBehaviour
{
    public int number;
    public bool canClick = false;
    private bool isCollected = false;

    public TextMeshPro numberText;

    [Header("Dialogue")]
    public AlgoDialogue algoDialogue; 

    void Start()
    {
        if (numberText != null)
            numberText.gameObject.SetActive(false);
    }

    public void SetNumber(int n)
    {
        number = n;

        if (numberText != null)
            numberText.text = number.ToString();
    }

    void OnMouseDown()
    {
        if (!canClick || isCollected) return;

        isCollected = true;
        canClick = false;

        if (numberText != null)
            numberText.gameObject.SetActive(true);

        // ⭐ เก็บเลขเข้าระบบ
        AlgoPuzzleManager.Instance.CollectNumber(number);

        // ⭐ ให้ NPC พูด
        if (algoDialogue != null)
            algoDialogue.ShowFoundNumberDialogue(number);

        Debug.Log("คลิกหนังสือ เลข = " + number);
    }
}
