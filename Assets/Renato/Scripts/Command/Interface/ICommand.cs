using UnityEngine;

public interface ICommand
{
    void Execute();
    void Undo();
}

public class KeyCommand 
{
    public KeyCode key;
    public ICommand command;
}

public interface IAxisCommand 
{
    float GetAxisValue();
}
