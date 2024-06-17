using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LoadingScreenHandler : MonoBehaviour
{
    public string targetScene;
    [SerializeField] private float transitionTime = 0.5f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
           DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator StartSceneTransition() {
        canvasGroup = GetComponent<CanvasGroup>();
        // FadeIn();
        // yield return new WaitForSeconds(transitionTime);

         for (int i = 0; i < 100; i++)
        {
            canvasGroup.alpha += 0.01f;
            yield return new WaitForSeconds(transitionTime / 100);
        }


        SceneManager.LoadScene(targetScene);
        // FadeOut();

          for (int i = 0; i < 100; i++)
        {
            canvasGroup.alpha -= 0.01f;
            yield return new WaitForSeconds(transitionTime / 100);
        }


        yield return new WaitForSeconds(transitionTime);
        Destroy(this.gameObject);
    }

    public void ToScene() {
        StartCoroutine(StartSceneTransition());
    }

    // void FadeIn() {
    //     canvasGroup.DOFade(1.0f, transitionTime).Play();
    // }

    // void FadeOut() {
    //     canvasGroup.DOFade(0.0f, transitionTime).Play();
    // }
}
