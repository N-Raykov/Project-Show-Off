using UnityEngine;
using UnityEngine.Events;

public class Activateable : MonoBehaviour
{
    [SerializeField] protected bool activated = false;
    protected ParticleSystem particles;
    protected SoundEffectType soundEffectType;
    protected string soundIdentifier;

    public void Activate()
    {
        if(activated == false)
        {
            EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(soundEffectType, transform.position, soundIdentifier));
            activated = true;
            particles.Play();
            //Debug.Log("Fan activated");
        }
    }

    public void DeActivate()
    {
        if (activated == true)
        {
            EventBus<StopLoopingSoundEffect>.Publish(new StopLoopingSoundEffect(soundIdentifier));
            activated = false;
            particles.Stop();
        }
    }
}
