using UnityEngine;

public class Event
{

}

public class CollectibleGathered : Event
{
    public readonly int value;

    public CollectibleGathered(int pValue)
    {
        value = pValue;
    }
}

public class SoundEffectPlayed : Event
{
    public readonly SoundEffectType soundEffectType;

    public SoundEffectPlayed(SoundEffectType pSoundEffectType)
    {
        soundEffectType = pSoundEffectType;
    }
}

public class SoundEffectVolumeChanged : Event
{
    public readonly SoundEffectType soundEffectType;
    public readonly float volume;

    public SoundEffectVolumeChanged(SoundEffectType pSoundEffectType, float pVolume)
    {
        soundEffectType = pSoundEffectType;
        volume = pVolume;
    }
}

public class PositionBroadcasted : Event
{
    public readonly Vector3 position;

    public PositionBroadcasted(Vector3 pPosition)
    {
        position = pPosition;
    }
}