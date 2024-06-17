using UnityEngine;

public class FanHover : Activateable
{
    [SerializeField] private float power = 10;

    private Rigidbody rb;
    private string playerTag = "Player";

    public void SetState(bool state)
    {
        activated = state;

        //A bit messy but only meant for debugging purposes
        gameObject.GetComponent<MeshRenderer>().enabled = state;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == playerTag) {
            rb = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == playerTag) {
            rb = null;
        }
    }

    private void FixedUpdate() {
        if (rb != null && activated) {
            rb.velocity += gameObject.transform.up * power;
        }
    }

    private void Start()
    {
        soundEffectType = SoundEffectType.Wind;
        soundIdentifier = System.Guid.NewGuid().ToString();
        SetState(activated);
        particles = GetComponentInChildren<ParticleSystem>();
        if (activated)
        {
            EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(soundEffectType, transform.position, soundIdentifier));
            particles.Play();
        }
    }
}
