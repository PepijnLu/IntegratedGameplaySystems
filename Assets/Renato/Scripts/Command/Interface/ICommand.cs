using UnityEngine;

public interface ICommand
{
    abstract void Execute();
    abstract void Undo();
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


/// <summary>
/// Interface for ScriptableObjects
/// </summary>
public interface IIdentifiable
{
    string Name { get; }
    Sprite Icon { get; }
    bool IsAdded { set; }
}

