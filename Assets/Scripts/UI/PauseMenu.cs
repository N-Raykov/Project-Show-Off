using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectVolumeSlider;
    [SerializeField] private AudioMixer masterMixer;

    private bool isActive;
    private string menuSceneName = "MainMenu";
    private string musicVolumeParamName = "musicVolume";
    private string menuSceneNameParamName = "soundEffectVolume";

    public void ContinueBtnPressed()
    {
        CloseMenu();
    }

    public void ResetFromLastCheckpointBtnPressed()
    {

    }

    public void QuitBtnPressed()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void MusicSliderValueChanged()
    {
        masterMixer.SetFloat(musicVolumeParamName, musicVolumeSlider.value);
    }

    public void SoundEffectSliderValueChanged()
    {
        masterMixer.SetFloat(menuSceneNameParamName, soundEffectVolumeSlider.value);
    }

    private void OnEnable()
    {
        reader.openCloseMenuEventPerformed += OnOptionsPressed;
    }

    private void OnDisable()
    {
        reader.openCloseMenuEventPerformed -= OnOptionsPressed;
    }

    private void Awake()
    {
        CloseMenu();
    }

    private void OnOptionsPressed()
    {
        if (isActive)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    private void OpenMenu()
    {
        transform.localScale = new Vector3(1, 1, 1);
        Time.timeScale = 0;
        isActive = true;
    }

    private void CloseMenu()
    {
        transform.localScale = new Vector3(0, 0, 0);
        Time.timeScale = 1;
        isActive = false;
    }
}
