using System;

/// <summary>
/// This class is used for receiving and sending events from different parts of the code base
/// For publishing event: EventBus<CLASS>.Publish(new CLASS());
/// For listening: EventBus<CLASS>.OnEvent += METHOD;
/// Every event gets its own class which contains all variables that need to be passed
/// </summary>
public class EventBus<T> where T : Event
{
    public static event Action<T> OnEvent;

    public static void Publish(T pEvent)
    {
        OnEvent?.Invoke(pEvent);
    }
}