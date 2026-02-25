using UnityEngine;
using System.Collections;

public class MoveForwardCommand : IWalkerCommand
{
    public IEnumerator Execute(CodeWalkerController walker)
    {
        yield return walker.MoveForward();
    }

    public string GetCode()
    {
        return "moveForward();";
    }
}