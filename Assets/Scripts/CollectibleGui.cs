using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CollectibleGui : MonoBehaviour
{
    [SerializeField] private float scaleIncrease = 1.5f;
    [SerializeField] private float speed = 75f;
    [SerializeField] private string text = "Candy: ";

    TextMeshProUGUI collectibleTMPro;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private int collectibleCount = 0;
    private bool isLerping = false;

    private void OnEnable()
    {
        EventBus.StartListening<int>(EventBusEnum.EventName.CollectibleGathered, OnCollectibleGathered);
    }

    private void OnDisable()
    {
        EventBus.StopListening<int>(EventBusEnum.EventName.CollectibleGathered, OnCollectibleGathered);
    }

    private void Start()
    {
        collectibleTMPro = GetComponent<TextMeshProUGUI>();
        if (collectibleTMPro == null)
        {
            throw new System.Exception("There is no TextMeshProUGUI component.");
        }

        originalScale = transform.localScale;
        targetScale = originalScale * scaleIncrease;
        collectibleTMPro.SetText(text + collectibleCount);
    }

    void Update()
    {
        if(isLerping)
            LerpScale();
    }

    private void OnCollectibleGathered(int pValue)
    {
        collectibleCount++;
        collectibleTMPro.SetText(text + collectibleCount);
        StartLerp();
    }

    private void LerpScale()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);

        if(transform.localScale == targetScale)
        {
            transform.localScale = originalScale;
            isLerping = false;
            //Debug.Log("STOP LERP");
        }
    }

    private void StartLerp()
    {
        transform.localScale = originalScale;
        isLerping = true;
        //Debug.Log("START LERP");
    }
}
