using UnityEngine;
using UnityEngine.Events;

public class Activateable : MonoBehaviour
{
    [SerializeField] protected bool activated = false;
    protected ParticleSystem particles;

    public void Activate()
    {
        if(activated == false)
        {
            EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Wind));
            activated = true;
            particles.Play();
            //Debug.Log("Fan activated");
        }
    }
}
