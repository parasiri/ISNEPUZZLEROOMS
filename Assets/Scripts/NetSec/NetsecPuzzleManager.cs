using UnityEngine;

public class NetsecPuzzleManager : MonoBehaviour
{
    public static NetsecPuzzleManager Instance;
    public bool PuzzleSolved { get; private set; } = false;


    [Header("Game Data")]
    public string currentPlaintext;
    public string currentKey;
    public string correctEncrypted;

    public bool PuzzleStarted { get; private set; } = false; 

    [Header("Candidates")]
    public string[] possiblePlaintexts =
    {
        "SEC",
        "NET",
        "ISNE",
        "HELLO",
        "CODE",
        "PLAY",
        "NOTE",
        "COM"
    };

    public string[] possibleKeys =
    {
        "KEY",
        "SAFE",
        "LOCK",
        "HI",
        "GO"
    };

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartPuzzle()
    {
        PuzzleStarted = true; // ⭐ สำคัญ

        currentPlaintext =
            possiblePlaintexts[Random.Range(0, possiblePlaintexts.Length)];

        currentKey =
            possibleKeys[Random.Range(0, possibleKeys.Length)];

        correctEncrypted =
            PlayfairCipher.Encrypt(currentPlaintext, currentKey);

        Debug.Log("PLAINTEXT: " + currentPlaintext);
        Debug.Log("KEY: " + currentKey);
        Debug.Log("ANSWER: " + correctEncrypted);
    }

    public bool CheckAnswer(string playerAnswer)
    {
        return playerAnswer.ToUpper() == correctEncrypted;
    }

    public void MarkSolved()
    {
        PuzzleSolved = true;
        Debug.Log("PUZZLE SOLVED → DOOR UNLOCKED");
    }

}
