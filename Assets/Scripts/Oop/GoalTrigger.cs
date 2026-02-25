using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private Renderer rend;
    private bool activated = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;   // เริ่มต้นสีแดง
    }

    void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            rend.material.color = Color.green;  // เปลี่ยนเป็นเขียว

            Debug.Log("Checkpoint Reached!");

            OOPPuzzleManager.Instance.PuzzleCompleted();
        }
    }
}