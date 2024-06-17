using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonEffects : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private string gameSceneName;
    [SerializeField] private GameObject loadingScreenPrefab;

    private LoadingScreenHandler loadingScreenHandler;

    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        reader.abilityEventPerformed += OnAnyButtonPressed;
        reader.interactEventPerformed += OnAnyButtonPressed;
        reader.jumpEventPerformed += OnAnyButtonPressed;
    }

    private void OnDisable()
    {
        reader.abilityEventPerformed -= OnAnyButtonPressed;
        reader.interactEventPerformed -= OnAnyButtonPressed;
        reader.jumpEventPerformed -= OnAnyButtonPressed;
    }

    private void OnAnyButtonPressed()
    {
        // SceneManager.LoadScene(gameSceneName);
        loadingScreenHandler = Instantiate(loadingScreenPrefab).GetComponent<LoadingScreenHandler>();
        loadingScreenHandler.targetScene = gameSceneName;
        loadingScreenHandler.ToScene();
    }
}
