using UnityEngine;

public class AxisCommand : IAxisCommand
{
    private readonly string Value = "";

    public AxisCommand(string value) 
    {
        Value = value;
    }
    
    public float GetAxisValue() => Input.GetAxisRaw(Value);
}
