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
    public Vector3 position;
    public string identifier;
    public SoundEffectPlayed(SoundEffectType pSoundEffectType, Vector3 pPosition, string pIdentifier = "")
    {
        soundEffectType = pSoundEffectType;
        position = pPosition;
        identifier = pIdentifier;
    }
}

public class StopLoopingSoundEffect : Event
{
    public string identifier;

    public StopLoopingSoundEffect(string pPidentifier = "")
    {
        identifier = pPidentifier;
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