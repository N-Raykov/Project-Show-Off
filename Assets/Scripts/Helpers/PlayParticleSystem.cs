using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    void ActivateParticleSystem()
    {
        particles.Play();
    }

    void StopParticleSystem()
    {
        particles.Stop();
    }
}
