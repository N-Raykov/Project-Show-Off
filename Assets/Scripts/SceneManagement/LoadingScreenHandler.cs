using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LoadingScreenHandler : MonoBehaviour
{
    public string targetScene;

    [SerializeField] private float transitionSpeed = 2.5f;

    private CanvasGroup canvasGroup;

    public void ToScene()
    {
        StartCoroutine(StartSceneTransition());
    }

    private void Awake()
    {
           DontDestroyOnLoad(this.gameObject);
    }

    private IEnumerator StartSceneTransition() {
        canvasGroup = GetComponent<CanvasGroup>();
        // FadeIn();
        // yield return new WaitForSeconds(transitionTime);

         while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * transitionSpeed;
            yield return new WaitForSeconds( Time.deltaTime);
        }


        SceneManager.LoadScene(targetScene);
        // FadeOut();

         while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -= Time.deltaTime * transitionSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Destroy(this.gameObject);
    }


    // void FadeIn() {
    //     canvasGroup.DOFade(1.0f, transitionTime).Play();
    // }

    // void FadeOut() {
    //     canvasGroup.DOFade(0.0f, transitionTime).Play();
    // }
}
