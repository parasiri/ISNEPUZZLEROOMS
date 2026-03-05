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

    //private List<string> commands = new List<string>();
    private List<IWalkerCommand> commands = new List<IWalkerCommand>();
    private bool isRunning = false;

    private bool hasCollided = false;
    private Rigidbody rb;

    private bool reachedGoal = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
    }

    // ================= ADD COMMAND =================

    //public void AddCommand(string command)
    //{
    //    if (isRunning) return;

    //    commands.Add(command);
    //    Debug.Log("Added: " + command);
    //    UICommandDisplay.Instance.UpdateCommandText(commands);
    //}
    public void AddCommand(IWalkerCommand command)
    {
        if (isRunning) return;

        commands.Add(command);

        if (UICommandDisplay.Instance != null)
            UICommandDisplay.Instance.UpdateCommandText(GetCommandCodes());
    }
    List<string> GetCommandCodes()
    {
        List<string> codes = new List<string>();

        foreach (var cmd in commands)
            codes.Add(cmd.GetCode());

        return codes;
    }

    // ================= RUN =================

    public void RunCommands()
    {
        if (isRunning || commands.Count == 0) return;
        reachedGoal = false;
        StartCoroutine(ExecuteCommands());
    }

    //IEnumerator ExecuteCommands()
    //{
    //    isRunning = true;

    //    foreach (string cmd in commands)
    //    {
    //        if (cmd == "Forward")
    //            yield return MoveForward();

    //        if (cmd == "Left")
    //            transform.Rotate(0, -90, 0);

    //        if (cmd == "Right")
    //            transform.Rotate(0, 90, 0);


    //        yield return new WaitForSeconds(0.3f);
    //    }

    //    isRunning = false;

    //    CheckGoal();
    //}

    IEnumerator ExecuteCommands()
    {
        isRunning = true;
        hasCollided = false;

        UICommandDisplay.Instance.gameObject.SetActive(false);

        foreach (var cmd in commands)
        {
            if (hasCollided || reachedGoal)
                yield break;

            yield return cmd.Execute(this);
        }

        isRunning = false;

        if (!reachedGoal)
        {
            ResetPositionOnly();

            if (UICommandDisplay.Instance != null)
                UICommandDisplay.Instance.gameObject.SetActive(true);
        }
    }

    //public IEnumerator MoveForward()
    //{
    //    Vector3 target = transform.position + transform.forward * moveDistance;

    //    while (Vector3.Distance(transform.position, target) > 0.01f)
    //    {
    //        transform.position = Vector3.MoveTowards(
    //            transform.position,
    //            target,
    //            moveSpeed * Time.deltaTime);
    //        yield return null;
    //    }
    //}
    public IEnumerator MoveForward(float distance)
    {
        Vector3 start = rb.position;
        Vector3 target = start + transform.forward * distance;

        float elapsed = 0f;
        float duration = distance / moveSpeed;

        while (elapsed < duration)
        {
            if (hasCollided || reachedGoal)
                yield break;

            elapsed += Time.fixedDeltaTime;

            Vector3 newPos = Vector3.Lerp(start, target, elapsed / duration);
            rb.MovePosition(newPos);

            CheckGoal();

            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(target);

        CheckGoal();
    }


    // ================= RESET =================

    public void ResetCommands()
    {
        StopAllCoroutines();
        commands.Clear();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = startPosition;
        rb.rotation = startRotation;

        isRunning = false;

        if (UICommandDisplay.Instance != null)
            UICommandDisplay.Instance.UpdateCommandText(GetCommandCodes());
    }
    void ResetPositionOnly()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = startPosition;
        rb.rotation = startRotation;
    }

    // ================= CHECK GOAL =================

    void CheckGoal()
    {
        if (goalPoint == null) return;

        if (Vector3.Distance(transform.position, goalPoint.position) < 0.5f)
        {
            Debug.Log("SUCCESS!");

            reachedGoal = true;
            isRunning = false;

            StopAllCoroutines();

            // ปิด Command Display
            if (UICommandDisplay.Instance != null)
                UICommandDisplay.Instance.gameObject.SetActive(false);

            // ปิด Popup เลือกคำสั่ง (ตัวนี้แหละที่ต้องปิด)
            if (CommandSelectionUI.Instance != null)
                CommandSelectionUI.Instance.ClosePanel();

            OOPPuzzleManager.Instance.PuzzleCompleted();
        }
    }

    // ================= Collision =================
    void OnCollisionEnter(Collision collision)
    {
        if (!isRunning) return;

        if (collision.transform == goalPoint)
            return;

        Debug.Log("Collided with: " + collision.gameObject.name);

        hasCollided = true;

        // 🔥 หยุดการทำงานทั้งหมด
        StopAllCoroutines();

        // 🔥 รีเซ็ตตำแหน่ง
        ResetPositionOnly();

        // 🔥 เปิด command display กลับมา
        if (UICommandDisplay.Instance != null)
            UICommandDisplay.Instance.gameObject.SetActive(true);

        isRunning = false;
    }

    IEnumerator HandleCollisionReset()
    {
        yield return new WaitForSeconds(0.2f);

        ResetCommands();

        hasCollided = false;
    }
}
