using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject loadingScreenPrefab;

    private string playerTag = "Player";
    private LoadingScreenHandler loadingScreenHandler;

    //I don't like this code but it works for a quick prototype
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
