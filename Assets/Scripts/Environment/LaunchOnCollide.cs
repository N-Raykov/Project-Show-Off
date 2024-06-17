using UnityEngine;

public class LaunchOnCollide : MonoBehaviour
{
    [SerializeField] private float launchPower = 5f;

    private string playerTag = "Player";

   void OnCollisionEnter(Collision other) {
    
    if (other.gameObject.tag == playerTag) {
        other.rigidbody.velocity = transform.up * launchPower;
        EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Trampoline, transform.position));
    }
   }
 }
