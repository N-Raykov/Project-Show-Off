using System.Collections;
using UnityEngine;


public class TriggerScene : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private GameObject loadingScreenPrefab;
    [SerializeField] private string sceneName;

    private LoadingScreenHandler loadingScreenHandler;
    private string playerTag = "Player";

    //Puts a loading screen in front when loading level
    void delayedTrigger()
    {
            loadingScreenHandler = Instantiate(loadingScreenPrefab).GetComponent<LoadingScreenHandler>();
            loadingScreenHandler.targetScene = sceneName;
            loadingScreenHandler.ToScene();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {
            reader.SetEnabledActionMap(false, false, true);
            //Show loading screen with a slight delay
            Invoke("delayedTrigger", 1.0f);
        }

    }
}
