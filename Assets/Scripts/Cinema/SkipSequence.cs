using UnityEngine;
using UnityEngine.Playables;

public class SkipSequence : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    [SerializeField] protected PlayerInputReader reader;

    private bool isPlaying = true;

    private void OnEnable() {
        reader.pauseEventPerformed += Skip;
    }

    private void OnDisable() {
        reader.pauseEventPerformed -= Skip;
    }

    private void Update()
    {
        if(isPlaying)
        {
            if (director.state != PlayState.Playing)
            {
                isPlaying = false;
                CutsceneOver();
            }
        }         
    }

    private void Skip() {
        if (director == null)
            return;

        CutsceneOver();
    }

    private void CutsceneOver()
    {
        director.Stop();
        reader.SetEnabledActionMap(true, false);
    }
}
