using UnityEngine;
using UnityEngine.Events;

public class Activateable : MonoBehaviour
{
    [SerializeField] protected bool activated = false;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SoundEffectType soundEffectType;
    protected ParticleSystem particles;
    protected string soundIdentifier;

    public void Activate()
    {
        if(activated == false)
        {
            EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(soundEffectType, transform.position, soundIdentifier));
            activated = true;
            particles.Play();
            animator.SetTrigger("Activated");
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
            animator.SetTrigger("Deactivated");
        }
    }
}
