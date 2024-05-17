using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetTextToTextBox : MonoBehaviour
{
    [TextArea(2, 3)]
    [SerializeField] private string message = "Press BUTTONPROMPT to interact.";

    [Header("Setup for sprites")]
    [SerializeField] private ListOfTmpSprites listOfTmpSprites;
    [SerializeField] private DeviceType deviceType;

    private CustomPlayerInput _playerInput;
    private TMP_Text _textBox;

    private void Awake()
    {
        _playerInput = new CustomPlayerInput();
        _textBox = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        SetText();
    }

    [ContextMenu("Set Text")]
    private void SetText()
    {
        if((int)deviceType > listOfTmpSprites.SpriteAssets.Count - 1)
        {
            Debug.Log($"Missing Sprite Asset for {deviceType}");
            return;
        }

        _textBox.text = CompleteTextWithButtonSprite.ReadAndReplaceBinding(message, _playerInput.Player.Interact.bindings[(int)deviceType], listOfTmpSprites.SpriteAssets[(int)deviceType]);
    }

    public enum DeviceType
    { 
        Keyboard = 0,
        GamePad = 1
    }
}
