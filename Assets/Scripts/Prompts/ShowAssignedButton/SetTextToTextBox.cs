using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class SetTextToTextBox : MonoBehaviour
{
    [TextArea(2, 3)]
    public string message = "Press BUTTONPROMPT to interact.";

    [Header("Set Action")]
    public ActionType actionType;

    private DeviceType deviceType;
    private CustomPlayerInput _playerInput;
    private TMP_Text _textBox;

    private void Awake()
    {
        _playerInput = new CustomPlayerInput(); //CustomPlayerInput class is just the class the new input system makes for the control scheme we will be using.
        _textBox = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        CheckForController();
        SetText();
    }

    private void CheckForController()
    {
        Gamepad gamepad = Gamepad.current;
        if(gamepad == null)
        {
            deviceType = DeviceType.Keyboard;
        }
        else
        {
            deviceType = DeviceType.GamePad;
        }
    }
    
    [ContextMenu("Set Text")]
    private void SetText()
    {
        InputAction action = _playerInput.FindAction(actionType.ToString());
        // Debug.Log(action);
        
        _textBox.text = CompleteTextWithButtonSprite.ReadAndReplaceBinding(message, action.bindings[(int)deviceType], (int)deviceType);
    }

    //In the input menu make sure that keyboard is the first input binding and gamepad is the second. Using different control schemes would be better
    //but that would require changing a shit ton of code everywhere so let's stick with this.
    private enum DeviceType
    { 
        Keyboard = 0,
        GamePad = 1
    }
}
