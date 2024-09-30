using System.Collections.Generic;
using UnityEngine;

public class R_InputHandler 
{
    public List<KeyCommand> keyCommands = new();
    public bool keyPressed;

    public ICommand HandleKeyInput() 
    {
        // Loop through all the key commands
        foreach (KeyCommand keyCommand in keyCommands)
        {
            if(Input.GetKeyDown(keyCommand.key)) 
            {
                keyPressed = true;
                Debug.Log($"Key input detected: {keyCommand.key}");
                UnBindInput(keyCommand.key);
                return keyCommand.command;
            }
            
            if(Input.GetKeyUp(keyCommand.key)) 
            {
                keyPressed = false;
                Debug.Log($"Key input released: {keyCommand.key}");
                UnBindInput(keyCommand.key);
                return null;
            }
        }

        return null;
    }

    public ICommand HandleContinuousKeyInput() 
    {
        foreach (KeyCommand keyCommand in keyCommands)
        {
            if(Input.GetKey(keyCommand.key)) 
            {
                keyPressed = true;
                keyCommand.command.Execute();

                Debug.Log($"Continuous key input detected: {keyCommand.key}");
            }

            if(Input.GetKeyUp(keyCommand.key))  
            {
                keyPressed = false;
                keyCommand.command.Undo();

                Debug.Log($"Continuous key input released: {keyCommand.key}");
            }
        }
        return null;
    }

    public ICommand HandleMovement
    (
        IAxisCommand horizontalAxis,
        IAxisCommand verticalAxis,
        Transform transform,
        float movementSpeed
    ) 
    {
        if(horizontalAxis == null || verticalAxis == null) return null;

        float x = horizontalAxis.GetAxisValue();
        float y = verticalAxis.GetAxisValue();

        Vector2 direction = movementSpeed * Time.deltaTime * new Vector2(x, y);
        transform.Translate(direction);
        
        return null;
    }

    public void BindInputToCommand(KeyCode keyCode, ICommand command) 
    {
        keyCommands.Add(new KeyCommand() 
        {
            key = keyCode,
            command = command
        }); 
    }

    public void UnBindInput(KeyCode keyCode) 
    {
        List<KeyCommand> keys = keyCommands.FindAll(k => k.key == keyCode);
    }
}
