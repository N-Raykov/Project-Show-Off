using UnityEngine;

public class MainMenuButtonEffects : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private GameObject loadingScreenPrefab;
    [SerializeField] private string gameSceneName;

    private LoadingScreenHandler loadingScreenHandler;

    public void LoadGameScene()
    {
        OnAnyButtonPressed();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        reader.continueEventPerformed += OnAnyButtonPressed;
    }

    private void OnDisable()
    {
        reader.continueEventPerformed -= OnAnyButtonPressed;
    }

    private void OnAnyButtonPressed()
    {
        loadingScreenHandler = Instantiate(loadingScreenPrefab).GetComponent<LoadingScreenHandler>();
        loadingScreenHandler.targetScene = gameSceneName;
        loadingScreenHandler.ToScene();
    }
}
