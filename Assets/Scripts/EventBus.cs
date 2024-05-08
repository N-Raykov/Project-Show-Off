using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBusEnum
{
    public enum EventName
    {
        POG
    }
}

    //Trigger 
    //EventBus.TriggerEvent<type example: int/string/custom type>(EventBusEnum.EventName.enum from EventBusEnums class, value);
    //Start Listening 
    //EventBus.StartListening<type example: int/string/custom type>(EventBusEnum.EventName.enum from EventBusEnums class, method that's listening);
    //Stop Listening 
    //EventBus.StopListening<type example: int/string/custom type>(EventBusEnum.EventName.enum from EventBusEnums class, method that's listening);


public class EventBus : MonoBehaviour
{
    Hashtable eventHash = new Hashtable();

    private static EventBus eventBus;
    public static EventBus instance
    {
        get
        {
            if (!eventBus)
            {
                eventBus = FindObjectOfType(typeof(EventBus)) as EventBus;

                if (!eventBus)
                {
                    Debug.LogError("There needs to be one active EventBus script on a GameObject in your scene.");
                }
                else
                {
                    eventBus.Init();
                }
            }

            return eventBus;
        }
    }

    void Init()
    {
        if (eventBus.eventHash == null)
        {
            eventBus.eventHash = new Hashtable();
        }
    }

    private static string GetKey<T>(EventBusEnum.EventName eventName)
    {
        Type type = typeof(T);
        string key = type.ToString() + eventName.ToString();
        return key;
    }

    public static void StartListening<T>(EventBusEnum.EventName eventName, UnityAction<T> listener)
    {
        UnityEvent<T> thisEvent = null;

        string b = GetKey<T>(eventName);

        if (instance.eventHash.ContainsKey(b))
        {
            thisEvent = (UnityEvent<T>)instance.eventHash[b];
            thisEvent.AddListener(listener);
            instance.eventHash[eventName] = thisEvent;
        }
        else
        {
            thisEvent = new UnityEvent<T>();
            thisEvent.AddListener(listener);
            instance.eventHash.Add(b, thisEvent);

        }
    }

    public static void StopListening<T>(EventBusEnum.EventName eventName, UnityAction<T> listener)
    {
        if (eventBus == null) return;
        UnityEvent<T> thisEvent = null;
        string key = GetKey<T>(eventName);
        if (instance.eventHash.ContainsKey(key))
        {
            thisEvent = (UnityEvent<T>)instance.eventHash[key];
            thisEvent.RemoveListener(listener);
            instance.eventHash[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent<T>(EventBusEnum.EventName eventName, T val)
    {
        UnityEvent<T> thisEvent = null;
        string key = GetKey<T>(eventName);
        if (instance.eventHash.ContainsKey(key))
        {
            thisEvent = (UnityEvent<T>)instance.eventHash[key];
            thisEvent.Invoke(val);
        }
    }
}
