using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrompt : Prompt
{
    protected override void OnEnable()
    {
        switch (actionType)
        {
            case ActionTypes.ActionType.Interact: reader.interactEventPerformed += PerformInteraction;
                break;
            case ActionTypes.ActionType.Ability:
                reader.abilityEventPerformed += PerformInteraction;
                break;
            case ActionTypes.ActionType.Jump:
                reader.jumpEventPerformed += PerformInteraction;
                break;
            default:
                Debug.LogError("This switch case doesn't have an assigned reader Action for the corresponding ActionType");
                break;
        }
    }

    protected override void OnDisable()
    {
        switch (actionType)
        {
            case ActionTypes.ActionType.Interact:
                reader.interactEventPerformed -= PerformInteraction;
                break;
            case ActionTypes.ActionType.Ability:
                reader.abilityEventPerformed -= PerformInteraction;
                break;
            case ActionTypes.ActionType.Jump:
                reader.jumpEventPerformed -= PerformInteraction;
                break;
            default:
                Debug.LogError("This switch case doesn't have an assigned reader Action for the corresponding ActionType");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && prompt != null)
        {
            prompt.SetActive(true);
            UIManager.instance.ChangeTMProText(prompt, message, actionType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && prompt != null)
        {
            prompt.SetActive(false);
        }
    }

    protected override void PerformInteraction()
    {
        if (prompt.activeSelf) {
            gameObject.SetActive(false);
            Destroy(prompt);
        }
    }
}
