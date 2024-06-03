using UnityEngine;
using AYellowpaper.SerializedCollections;

public enum SoundEffectType
{
    PlayerJump,
    Interact,
    Ability,
    Wind,
    Collectible
}

public class SoundPlayer : MonoBehaviour
{
    [SerializedDictionary("SoundEffectEnum", "AudioSource")]
    [SerializeField] SerializedDictionary<SoundEffectType, AudioSource> soundEffects;

    private void OnEnable()
    {
        EventBus<SoundEffectPlayed>.OnEvent += OnSoundPlayed;
        EventBus<SoundEffectVolumeChanged>.OnEvent += OnSoundEffectVolumeChanged;
    }

    private void OnDisable()
    {
        EventBus<SoundEffectPlayed>.OnEvent -= OnSoundPlayed;
        EventBus<SoundEffectVolumeChanged>.OnEvent -= OnSoundEffectVolumeChanged;
    }
    private void OnSoundPlayed(SoundEffectPlayed pSoundEffectPlayed)
    {
        SoundEffectType soundEffectType = pSoundEffectPlayed.soundEffectType;

        soundEffects[soundEffectType].Play();
    }

    private void OnSoundEffectVolumeChanged(SoundEffectVolumeChanged pSoundEffectVolumeChanged)
    {
        SoundEffectType soundEffectType = pSoundEffectVolumeChanged.soundEffectType;
        float volume = pSoundEffectVolumeChanged.volume;

        soundEffects[soundEffectType].volume = volume;
    }
}
