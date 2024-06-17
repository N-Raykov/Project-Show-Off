using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkipSequence : MonoBehaviour
{

    [SerializeField] PlayableDirector director;
    [SerializeField] protected PlayerInputReader reader;

    void OnEnable() {
        reader.pauseEventPerformed += Skip;
    }

    void OnDisable() {
        reader.pauseEventPerformed -= Skip;
    }

    void Skip() {
        if (director == null) {
            return;
        }

        director.Stop();
    }
}
