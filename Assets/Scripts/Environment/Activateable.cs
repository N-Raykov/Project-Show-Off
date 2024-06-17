using UnityEngine;

public class Activateable : MonoBehaviour
{
    [SerializeField] protected bool activated = false;

    protected ParticleSystem particles;
    protected SoundEffectType soundEffectType;
    protected string soundIdentifier;

    public void Activate()
    {
        if(!activated)
        {
            EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(soundEffectType, transform.position, soundIdentifier));
            activated = true; //Debug.Log("Fan activated");
            particles.Play();            
        }
    }

    public void DeActivate()
    {
        if (activated)
        {
            EventBus<StopLoopingSoundEffect>.Publish(new StopLoopingSoundEffect(soundIdentifier));
            activated = false;
            particles.Stop();
        }
    }
}
