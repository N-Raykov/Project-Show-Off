using UnityEngine;
using UnityEngine.Events;

public class Activateable : MonoBehaviour
{
    [SerializeField] protected bool activated = false;
    protected ParticleSystem particles;
    protected SoundEffectType soundEffectType;
    protected string soundIdentifier;
    bool testing = false;
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
        if (testing == false)
        {
            EventBus<StopLoopingSoundEffect>.Publish(new StopLoopingSoundEffect(soundIdentifier));
            testing = true;
            particles.Stop();
        }
    }
}
