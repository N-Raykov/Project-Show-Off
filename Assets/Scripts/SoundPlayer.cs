using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private AudioSource playerJump;
    [SerializeField] private AudioSource interact;
    [SerializeField] private AudioSource ability;
    [SerializeField] private AudioSource wind;
    [SerializeField] private AudioSource collectible;

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

        switch (soundEffectType)
        {
            case SoundEffectType.PlayerJump:
                playerJump.Play();
                break;
            case SoundEffectType.Interact:
                interact.Play();
                break;
            case SoundEffectType.Ability:
                ability.Play();
                break;
            case SoundEffectType.Wind:
                wind.Play();
                break;
            case SoundEffectType.Collectible:
                collectible.Play();
                break;
        }
    }

    private void OnSoundEffectVolumeChanged(SoundEffectVolumeChanged pSoundEffectVolumeChanged)
    {
        SoundEffectType soundEffectType = pSoundEffectVolumeChanged.soundEffectType;
        float volume = pSoundEffectVolumeChanged.volume;

        switch (soundEffectType)
        {
            case SoundEffectType.PlayerJump:
                playerJump.volume = volume;
                break;
            case SoundEffectType.Interact:
                interact.volume = volume;
                break;
            case SoundEffectType.Ability:
                ability.volume = volume;
                break;
            case SoundEffectType.Wind:
                wind.volume = volume;
                break;
            case SoundEffectType.Collectible:
                collectible.volume = volume;
                break;
        }
    }
}
