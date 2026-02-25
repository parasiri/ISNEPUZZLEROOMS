using System.Collections;

public interface IWalkerCommand
{
    IEnumerator Execute(CodeWalkerController walker);
    string GetCode();
}