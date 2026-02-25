using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class TreeUIManager : MonoBehaviour
{
    [Header("Core")]
    public AlgoPuzzleManager puzzleManager;

    [Header("Tree Nodes")]
    public List<TreeNodeUI> treeNodes;

    [Header("Available Number Buttons")]
    public List<NumberButton> numberButtons;

    [Header("UI")]
    public TMP_Text treeTypeText;

    [Header("Panel")]
    public GameObject treePanel;


    //private int? selectedValue = null;
    //private Dictionary<TreeNodeUI, int> placedValues = new Dictionary<TreeNodeUI, int>();

    void OnEnable()
    {
        SetupNumberButtons();
        UpdateTreeTypeText();
    }

    void UpdateTreeTypeText()
    {
        treeTypeText.text =
            puzzleManager.currentTreeType == AlgoTreeType.BST
            ? "Tree Type: Binary Search Tree"
            : "Tree Type: Balanced Binary Tree";
    }

    void SetupNumberButtons()
    {
        var nums = puzzleManager.collectedNumbers;

        for (int i = 0; i < numberButtons.Count; i++)
        {
            if (i < nums.Count)
            {
                numberButtons[i].gameObject.SetActive(true);
                numberButtons[i].SetValue(nums[i], this);
            }
            else
            {
                numberButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // ====== Number ======
    //public void SelectNumber(int value)
    //{
    //    selectedValue = value;
    //    Debug.Log("Selected: " + value);
    //}

    // ====== Node ======
    //public void PlaceNumberOnNode(TreeNodeUI node)
    //{
    //    if (selectedValue == null)
    //    {
    //        Debug.Log("No number selected");
    //        return;
    //    }

    //    // ถ้า node มีเลขอยู่ → คืนปุ่มก่อน
    //    if (node.HasValue())
    //    {
    //        int oldValue = node.GetValue().Value;
    //        EnableButton(oldValue);
    //        placedValues.Remove(node);
    //    }

    //    node.SetValue(selectedValue.Value);
    //    placedValues[node] = selectedValue.Value;

    //    DisableButton(selectedValue.Value);
    //    selectedValue = null;
    //}

    void DisableButton(int value)
    {
        foreach (var btn in numberButtons)
            if (btn.value == value)
                btn.gameObject.SetActive(false);
    }

    void EnableButton(int value)
    {
        foreach (var btn in numberButtons)
            if (btn.value == value)
                btn.gameObject.SetActive(true);
    }

    // ====== Submit ======
    public void Submit()
    {
        List<int> order = new List<int>();

        foreach (var node in treeNodes)
        {
            if (node.HasValue())
                order.Add(node.GetValue().Value);
        }

        bool correct = puzzleManager.CheckAnswer(order);

        if (correct)
        {
            Debug.Log("✅ Correct!");

            // ปิด panel ต้นไม้ก่อน
            ClosePanel();

            // เล่น dialogue หลังตอบถูก
            if (puzzleManager.algoDialogue != null)
                puzzleManager.algoDialogue.ContinueAfterCorrectAnswer();
        }
        else
        {
            Debug.Log("❌ Wrong!");

            // เล่น dialogue ตอบผิด
            if (puzzleManager.algoDialogue != null)
                puzzleManager.algoDialogue.ShowWrongAnswer();
        }
    }


    //public void OnNodeClicked(TreeNodeUI node)
    //{
    //    // ถ้ามีเลขอยู่ → ถอนออก
    //    if (node.HasValue())
    //    {
    //        int value = node.GetValue().Value;
    //        node.Clear();
    //        EnableButton(value);
    //        selectedValue = null;
    //        Debug.Log("Removed value: " + value);
    //        return;
    //    }

    //    // ยังไม่ได้เลือกเลข
    //    if (selectedValue == null)
    //    {
    //        Debug.Log("No number selected");
    //        return;
    //    }

    //    // วางเลข
    //    node.SetValue(selectedValue.Value);
    //    DisableButton(selectedValue.Value);
    //    selectedValue = null;
    //}

    public static TreeUIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void PlaceValueByDrag(TreeNodeUI node, NumberButton button)
    {
        // ❌ ถ้ามีค่าอยู่แล้ว ไม่ให้วาง
        if (node.HasValue()) return;

        node.SetValue(button.value);

        button.placedSuccessfully = true;
        button.HideAfterPlaced();
    }



    public void ClosePanel()
    {
        treePanel.SetActive(false);
    }

  




}
