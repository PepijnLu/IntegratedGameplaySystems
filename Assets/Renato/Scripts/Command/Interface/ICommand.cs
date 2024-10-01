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

public interface IIdentifiable
{
    string Name { get; }
}

// public class NamableGameObject : INamable
// {
//     public GameObject GameObject { get; private set; }
//     public string Name => GameObject.name;

//     public NamableGameObject(GameObject gameObject)
//     {
//         GameObject = gameObject;
//     }
// }
