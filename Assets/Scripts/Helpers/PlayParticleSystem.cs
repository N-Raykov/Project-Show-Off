using UnityEngine;

public class PlayParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    private void ActivateParticleSystem()
    {
        particles.Play();
    }

    private void StopParticleSystem()
    {
        particles.Stop();
    }
}
