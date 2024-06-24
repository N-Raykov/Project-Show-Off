using UnityEngine;

public class CutsceneAnimationHelper : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;

    public void OnLastFrame()
    {
        reader.SetEnabledActionMap(true, false, false);
    }
}