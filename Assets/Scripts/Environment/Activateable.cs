using UnityEngine;
using UnityEngine.Events;

public class Activateable : MonoBehaviour
{
    [SerializeField] protected bool activated = false;

    public void Activate()
    {
        if(activated == false)
        {
            EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Wind));
            activated = true;
            //Debug.Log("Fan activated");
        }
    }
}
