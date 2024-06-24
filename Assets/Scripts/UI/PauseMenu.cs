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
    [SerializeField] private GameObject pauseMenu;

    private string menuSceneName = "MainMenu";
    private string musicVolumeParamName = "musicVolume";
    private string soundEffetcsParamName = "soundEffectVolume";

    public void ContinueBtnPressed()
    {
        CloseMenu();
    }

    public void ResetFromLastCheckpointBtnPressed()
    { 
        CloseMenu();
        FindObjectOfType<PlayerRespawn>().Respawn();
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
        masterMixer.SetFloat(soundEffetcsParamName, soundEffectVolumeSlider.value);
    }

    private void OnEnable()
    {
        reader.openMenuEventPerformed += OpenMenu;
        reader.closeMenuEventPerformed += CloseMenu;

    }

    private void OnDisable()
    {
        reader.openMenuEventPerformed -= OpenMenu;
        reader.closeMenuEventPerformed -= CloseMenu;
    }

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }

    private void OpenMenu()
    {
        pauseMenu.SetActive(true);
        float musicVolume;
        masterMixer.GetFloat(musicVolumeParamName, out musicVolume);
        musicVolumeSlider.value = musicVolume;
        float soundEffectsVolume;
        masterMixer.GetFloat(soundEffetcsParamName, out soundEffectsVolume);
        soundEffectVolumeSlider.value = soundEffectsVolume;
        reader.SetEnabledActionMap(false, true, false);
        //Time.timeScale = 0;
    }

    private void CloseMenu()
    {
        pauseMenu.SetActive(false);
        reader.SetEnabledActionMap(true, false, false);
        //Time.timeScale = 1;
    }
}
