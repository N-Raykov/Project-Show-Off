using UnityEngine.InputSystem;

public static class CompleteTextWithButtonSprite
{
    //Index 0 = Keyboard
    //Index 1 = Gamepad
    private static string[] deviceSprite = {
        "KeyboardButtons",
        "GamePadButtons"};

    //Reads the text and replaces the BUTTONPROMPT with the name for the corresponding sprite
    public static string ReadAndReplaceBinding(string textToDsiplay, InputBinding actionNeeded, int deviceType)
    {
        string stringButtonName = actionNeeded.ToString();
        //Debug.Log(actionNeeded);
        stringButtonName = RenameInput(stringButtonName, deviceType);
        textToDsiplay = textToDsiplay.Replace("BUTTONPROMPT", $"<sprite=\"{deviceSprite[deviceType]}\" name=\"{stringButtonName}\">");

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
