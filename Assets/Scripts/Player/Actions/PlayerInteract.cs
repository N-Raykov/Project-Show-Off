using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;

    private void OnEnable()
    {
        reader.interactEventPerformed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        reader.interactEventPerformed -= OnInteractPerformed;
    }

    private void OnInteractPerformed()
    {
        EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Interact, transform.position));
    }
}
