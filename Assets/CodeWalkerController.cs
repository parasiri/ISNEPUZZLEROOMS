using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CodeWalkerController : MonoBehaviour
{
    public static CodeWalkerController Instance;

    public Transform goalPoint;
    public float moveDistance = 1f;
    public float moveSpeed = 2f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private List<string> commands = new List<string>();
    private bool isRunning = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // ================= ADD COMMAND =================

    public void AddCommand(string command)
    {
        if (isRunning) return;

        commands.Add(command);
        Debug.Log("Added: " + command);
        UICommandDisplay.Instance.UpdateCommandText(commands);
    }

    // ================= RUN =================

    public void RunCommands()
    {
        if (isRunning || commands.Count == 0) return;
        StartCoroutine(ExecuteCommands());
    }

    IEnumerator ExecuteCommands()
    {
        isRunning = true;

        foreach (string cmd in commands)
        {
            if (cmd == "Forward")
                yield return MoveForward();

            if (cmd == "Left")
                transform.Rotate(0, -90, 0);

            if (cmd == "Right")
                transform.Rotate(0, 90, 0);


            yield return new WaitForSeconds(0.3f);
        }

        isRunning = false;

        CheckGoal();
    }

    IEnumerator MoveForward()
    {
        Vector3 target = transform.position + transform.forward * moveDistance;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime);
            yield return null;
        }
    }


    // ================= RESET =================

    public void ResetCommands()
    {
        StopAllCoroutines();
        commands.Clear();
        transform.position = startPosition;
        transform.rotation = startRotation;
        isRunning = false;

        UICommandDisplay.Instance.UpdateCommandText(commands);
    }

    // ================= CHECK GOAL =================

    void CheckGoal()
    {
        if (goalPoint == null) return;

        if (Vector3.Distance(transform.position, goalPoint.position) < 0.5f)
        {
            Debug.Log("SUCCESS!");
            OOPPuzzleManager.Instance.PuzzleCompleted();
        }
    }
}
