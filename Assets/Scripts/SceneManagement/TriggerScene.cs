using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject loadingScreenPrefab;

    private string playerTag = "Player";
    private LoadingScreenHandler loadingScreenHandler;

    //Puts a loading screen in front when loading level
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {
            // SceneManager.LoadScene(sceneName);
            loadingScreenHandler = Instantiate(loadingScreenPrefab).GetComponent<LoadingScreenHandler>();
            loadingScreenHandler.targetScene = sceneName;
            loadingScreenHandler.ToScene();
        }
    }
}
