using System;
using System.Reflection;
using UnityEngine;

public class TutorialPrompt : Prompt
{
    private EventInfo eventInfo;
    private Delegate eventHandler;
    private string eventName;

    private string playerTag = "Player";
    private string eventPerformed = "EventPerformed";
    private string performInteraction = "PerformInteraction";

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
        eventName = actionType.ToString() + eventPerformed;
        eventInfo = typeof(PlayerInputReader).GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);

        if (eventInfo != null)
        {
            MethodInfo methodInfo = GetType().GetMethod(performInteraction, BindingFlags.NonPublic | BindingFlags.Instance);
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

        eventName = actionType.ToString() + eventPerformed;
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
        if (other.CompareTag(playerTag) && prompt != null)
        {
            prompt.SetActive(true);
            UIManager.instance.ChangeTMProText(prompt, message, actionType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && prompt != null)
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
