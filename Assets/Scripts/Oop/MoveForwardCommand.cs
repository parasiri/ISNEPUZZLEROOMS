using UnityEngine;
using System.Collections;

public class MoveForwardCommand : IWalkerCommand
{
    private float distance;

    public MoveForwardCommand(float distance)
    {
        this.distance = distance;
    }

    public IEnumerator Execute(CodeWalkerController controller)
    {
        yield return controller.MoveForward(distance);
    }

    public string GetCode()
    {
        return "Forward(" + distance + ")";
    }
}