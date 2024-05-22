using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHideUI : MonoBehaviour
{
    [SerializeField] private PlayerInputReader playerInputReader;

    void Start() {
        playerInputReader.abilityEventPerformed += onInputPerformed;
    }

    void onInputPerformed() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }
}
