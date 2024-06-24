using UnityEngine;

public class TriggerScene : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private GameObject loadingScreenPrefab;
    [SerializeField] private string sceneName;

    private LoadingScreenHandler loadingScreenHandler;
    private string playerTag = "Player";

    //Puts a loading screen in front when loading level
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {
            reader.SetEnabledActionMap(false, false, true);
            loadingScreenHandler = Instantiate(loadingScreenPrefab).GetComponent<LoadingScreenHandler>();
            loadingScreenHandler.targetScene = sceneName;
            loadingScreenHandler.ToScene();
        }
    }
}
