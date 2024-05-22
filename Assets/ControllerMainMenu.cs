using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMainMenu : MonoBehaviour
{
   [SerializeField] private PlayerInputReader playerInputReader;

    void Start() {
        playerInputReader.interactEventPerformed += onInputPerformed;
    }

    void onInputPerformed() {
       SceneManager.LoadScene("LevelArt");
    }
}
