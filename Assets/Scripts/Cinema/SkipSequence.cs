using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkipSequence : MonoBehaviour
{

    [SerializeField] PlayableDirector director;
    [SerializeField] protected PlayerInputReader reader;

    void OnEnable() {
        reader.skipEventPerformed += Skip;
    }

    void OnDisable() {
        reader.skipEventPerformed -= Skip;
    }

    void Skip() {
        if (director == null) {
            return;
        }

        director.Stop();
    }
}
