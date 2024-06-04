using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrompt : Prompt
{
    private string eventName;
    private EventInfo eventInfo;
    private Delegate eventHandler;

    protected override void OnEnable()
    {
        SubscribeToEvent();
    }

    protected override void OnDisable()
    {
        UnsubscribeFromEvent();
    }

    private void SubscribeToEvent()
    {
        eventName = actionType.ToString() + "EventPerformed";
        eventInfo = typeof(PlayerInputReader).GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);

        if (eventInfo != null)
        {
            MethodInfo methodInfo = GetType().GetMethod("PerformInteraction", BindingFlags.NonPublic | BindingFlags.Instance);
            eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);
            eventInfo.AddEventHandler(reader, eventHandler);
        }
        else
        {
            Debug.LogError($"No event found with the name {eventName} in Reader class.");
        }
    }

    private void UnsubscribeFromEvent()
    {
        if (eventHandler == null) return;

        eventName = actionType.ToString() + "EventPerformed";
        eventInfo = typeof(PlayerInputReader).GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);

        if (eventInfo != null)
        {
            eventInfo.RemoveEventHandler(reader, eventHandler);
        }
        else
        {
            Debug.LogError($"No event found with the name {eventName} in Reader class.");
        }

        eventHandler = null;
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
            // Destroy(prompt);
            prompt.SetActive(false);
            EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Collectible, transform.position));
        }
    }
}
