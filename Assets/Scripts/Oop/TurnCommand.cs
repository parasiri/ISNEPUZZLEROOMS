using UnityEngine;
using System.Collections;

public class TurnCommand : IWalkerCommand
{
    private float angle;

    public TurnCommand(float angle)
    {
        this.angle = angle;
    }

    public IEnumerator Execute(CodeWalkerController walker)
    {
        walker.transform.Rotate(0, angle, 0);
        yield return new WaitForSeconds(0.2f);
    }

    public string GetCode()
    {
        if (angle > 0)
            return $"turnRight({angle});";
        else
            return $"turnLeft({Mathf.Abs(angle)});";
    }
}