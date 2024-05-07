using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonEffects : MonoBehaviour
{
    [SerializeField] private string gameSceneName;

    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
