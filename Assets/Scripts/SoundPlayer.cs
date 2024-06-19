using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public enum SoundEffectType
{
    PlayerJump,
    Interact,
    Ability,
    Wind,
    Collectible,
    Trampoline
}

public class SoundPlayer : MonoBehaviour
{
    [Header("SoundEffects")]
    [SerializedDictionary("SoundEffectEnum", "AudioSource")]
    [SerializeField] private SerializedDictionary<SoundEffectType, AudioSource> soundEffects;
    private Dictionary<string, AudioSource> activeAudioSources = new Dictionary<string, AudioSource>();


    [Header("Music")]
    [SerializeField] private AudioSource introMusicPart;
    [RangeFromAudioClip("introMusicPart")]
    [SerializeField] private float introMusicLength;

    [Space(20)]

    [SerializeField] private AudioSource loopingMusicPart;

    private string playLoopingMusic = "PlayLoopingMusic";

    private void OnEnable()
    {
        EventBus<SoundEffectPlayed>.OnEvent += OnSoundPlayed;
        EventBus<SoundEffectVolumeChanged>.OnEvent += OnSoundEffectVolumeChanged;
        EventBus<StopLoopingSoundEffect>.OnEvent += OnStopSoundEffect;
    }

    private void OnDisable()
    {
        EventBus<SoundEffectPlayed>.OnEvent -= OnSoundPlayed;
        EventBus<SoundEffectVolumeChanged>.OnEvent -= OnSoundEffectVolumeChanged;
        EventBus<StopLoopingSoundEffect>.OnEvent -= OnStopSoundEffect;
    }

    private void Start()
    {
        PlayIntroMusic();
    }

    private void OnSoundPlayed(SoundEffectPlayed pSoundEffectPlayed)
    {
        SoundEffectType soundEffectType = pSoundEffectPlayed.soundEffectType;
        Vector3 position = pSoundEffectPlayed.position;
        string identifier = pSoundEffectPlayed.identifier;
        
        if (string.IsNullOrEmpty(identifier))
        {
            identifier = System.Guid.NewGuid().ToString();
        }

        if (soundEffects.TryGetValue(soundEffectType, out AudioSource originalSource))
        {
            PlaySoundAtPosition(originalSource, position, identifier);
        }
    }

    private void PlaySoundAtPosition(AudioSource originalSource, Vector3 position, string identifier)
    {
        GameObject audioObject = new GameObject(identifier);
        audioObject.transform.position = position;
        audioObject.transform.parent = this.transform;
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        CopyAudioSource(originalSource, audioSource);

        audioSource.Play();

        activeAudioSources[identifier] = audioSource;

        if (!audioSource.loop)
        {
            Destroy(audioObject, audioSource.clip.length);
        }
    }

    private void OnSoundEffectVolumeChanged(SoundEffectVolumeChanged pSoundEffectVolumeChanged)
    {
        SoundEffectType soundEffectType = pSoundEffectVolumeChanged.soundEffectType;
        float volume = pSoundEffectVolumeChanged.volume;

        foreach (var audioSource in activeAudioSources.Values)
        {
            if (audioSource.clip == soundEffects[soundEffectType].clip)
            {
                audioSource.volume = volume;
            }
        }
    }

    private void OnStopSoundEffect(StopLoopingSoundEffect stopSoundEffect)
    {
        string identifier = stopSoundEffect.identifier;

        if (activeAudioSources.TryGetValue(identifier, out AudioSource audioSource))
        {
            StartCoroutine(FadeOutLoopingSound(audioSource));
            activeAudioSources.Remove(identifier);
        }
    }

    private void PlayIntroMusic()
    {
        if (introMusicPart != null && loopingMusicPart != null)
        {
            introMusicPart.Play();
            Invoke(playLoopingMusic, introMusicLength);
        }
        else
        {
            Debug.LogWarning("Intro or Looping Music AudioSource is not assigned.");
        }
    }

    private void PlayLoopingMusic()
    {
        introMusicPart.Stop();
        Debug.Log("It's looping!!!");
        loopingMusicPart.loop = true;
        loopingMusicPart.Play();
    }

    private IEnumerator FadeOutLoopingSound(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 1.0f;
            yield return null;
        }

        audioSource.Stop();
        Destroy(audioSource.gameObject);
    }

    private void CopyAudioSource(AudioSource original, AudioSource target)
    {
        target.clip = original.clip;
        target.volume = original.volume;
        target.pitch = original.pitch;
        target.spatialBlend = original.spatialBlend;
        target.minDistance = original.minDistance;
        target.maxDistance = original.maxDistance;
        target.loop = original.loop;
        target.playOnAwake = original.playOnAwake;
        target.outputAudioMixerGroup = original.outputAudioMixerGroup;
    }
}
