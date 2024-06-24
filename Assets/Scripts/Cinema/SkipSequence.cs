using UnityEngine;
using UnityEngine.Playables;

public class SkipSequence : MonoBehaviour
{
    [SerializeField] protected PlayerInputReader reader;

    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject ui;

    private bool isPlaying = true;

    void OnEnable() {
        reader.skipEventPerformed += Skip;
    }

    void OnDisable() {
        reader.skipEventPerformed -= Skip;
    }

    private void Update()
    {
        if(isPlaying)
        {
            ui.SetActive(false);
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
        ui.SetActive(true);
        director.Stop();
        reader.SetEnabledActionMap(true, false, false);
    }
}
