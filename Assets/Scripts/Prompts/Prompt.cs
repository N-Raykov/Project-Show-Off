using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Prompt : MonoBehaviour
{
    [SerializeField] protected GameObject prompt;
    [SerializeField] protected PlayerInputReader reader;

    [Header("Set Action")]
    [SerializeField] protected ActionType actionType;
    [TextArea(2, 3)]
    public string message = "Press BUTTONPROMPT to interact.";

    protected virtual void OnEnable()
    {
        reader.interactEventPerformed += PerformInteraction;
    }

    protected virtual void OnDisable()
    {
        reader.interactEventPerformed -= PerformInteraction;
    }

    protected virtual void PerformInteraction()
    {
        if(prompt == null)
        {
            return;
        }
        Debug.Log("Interaction performed!");
    }
}
