using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public static class CompleteTextWithButtonSprite
{
    public static string ReadAndReplaceBinding(string textToDsiplay, InputBinding actionNeeded, int deviceType)
    {
        string stringButtonName = actionNeeded.ToString();
        stringButtonName = RenameInput(stringButtonName, deviceType);
        textToDsiplay = textToDsiplay.Replace("BUTTONPROMPT", $"<sprite name=\"{stringButtonName}\">");

        return textToDsiplay;
    }

    private static string RenameInput(string stringButtonName, int deviceType)
    {
        int index = stringButtonName.IndexOf(':');

        if(index != -1)
        {
            stringButtonName = stringButtonName.Substring(index + 1);
        }


        switch (deviceType)
        {
            case 0: 
                stringButtonName = stringButtonName.Replace("<Keyboard>/", "Keyboard_");
                break;
            case 1: 
                stringButtonName = stringButtonName.Replace("<Gamepad>/", "Gamepad_");
                break;
            default:
                break;
        }

        return stringButtonName;
    }
}
