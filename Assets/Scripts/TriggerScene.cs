using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private string playerTag = "Player";

    //I don't like this code but it works for a quick prototype
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
