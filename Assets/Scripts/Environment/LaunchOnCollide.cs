using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchOnCollide : MonoBehaviour
{
    [SerializeField] private float launchPower = 5f;

   void OnCollisionEnter(Collision other) {
    
    if (other.gameObject.tag == "Player") {
        other.rigidbody.velocity = transform.up * launchPower;
        EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Trampoline, transform.position));
    }
   }
 }
