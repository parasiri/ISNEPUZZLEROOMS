using System.Collections.Generic;
using UnityEngine;

public enum AlgoTreeType
{
    BST,
    Balanced
}

public class AlgoPuzzleManager : MonoBehaviour
{
    public static AlgoPuzzleManager Instance;

    [Header("Collected Numbers")]
    public List<int> collectedNumbers = new List<int>();
    public int totalBooksRequired = 5;

    [Header("Tree Type")]
    public AlgoTreeType currentTreeType;

    [Header("Dialogue")]
    public AlgoDialogue algoDialogue;

    private bool treeInstructionShown = false; // ⭐ เพิ่มตรงนี้

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ================== Collect Number ==================
    public void CollectNumber(int num)
    {
        if (collectedNumbers.Contains(num))
            return;

        collectedNumbers.Add(num);
        Debug.Log($"Collected {collectedNumbers.Count}/{totalBooksRequired}");

        if (collectedNumbers.Count == totalBooksRequired && !treeInstructionShown)
        {
            treeInstructionShown = true;

            DecideTreeType();

            if (algoDialogue != null)
                algoDialogue.ShowTreeInstruction(currentTreeType);
        }
    }

    // ================== Random Tree Type ==================
    void DecideTreeType()
    {
        currentTreeType = (AlgoTreeType)Random.Range(0, 2);
        Debug.Log("Tree Type: " + currentTreeType);
    }

    // ================== CHECK ANSWER ==================
    public bool CheckAnswer(List<int> playerOrder)
    {
        TreeNode root = BuildTreeFromOrder(playerOrder);

        if (currentTreeType == AlgoTreeType.BST)
            return IsBST(root, int.MinValue, int.MaxValue);

        if (currentTreeType == AlgoTreeType.Balanced)
            return IsBalanced(root);

        return false;
    }

    // ================== TREE BUILD ==================
    TreeNode BuildTreeFromOrder(List<int> order)
    {
        TreeNode root = null;

        foreach (int n in order)
            root = InsertBST(root, n);

        return root;
    }

    TreeNode InsertBST(TreeNode root, int value)
    {
        if (root == null)
            return new TreeNode(value);

        if (value < root.value)
            root.left = InsertBST(root.left, value);
        else
            root.right = InsertBST(root.right, value);

        return root;
    }

    // ================== BST CHECK ==================
    bool IsBST(TreeNode node, int min, int max)
    {
        if (node == null) return true;

        if (node.value <= min || node.value >= max)
            return false;

        return IsBST(node.left, min, node.value) &&
               IsBST(node.right, node.value, max);
    }

    // ================== BALANCED CHECK ==================
    bool IsBalanced(TreeNode node)
    {
        if (node == null) return true;

        int lh = Height(node.left);
        int rh = Height(node.right);

        if (Mathf.Abs(lh - rh) > 1)
            return false;

        return IsBalanced(node.left) && IsBalanced(node.right);
    }

    int Height(TreeNode node)
    {
        if (node == null) return 0;

        return 1 + Mathf.Max(Height(node.left), Height(node.right));
    }
}
