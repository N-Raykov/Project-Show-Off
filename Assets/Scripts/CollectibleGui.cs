using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CollectibleGui : MonoBehaviour
{
    [SerializeField] private float scaleIncrease = 1.5f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private string text = "Candy: ";

    TextMeshProUGUI collectibleTMPro;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private int collectibleCount = 0;
    private bool isLerping = false;
    private float lerpStartTime;
    private float lerpDifference;

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

        lerpDifference = targetScale.magnitude - originalScale.magnitude;
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
        float lerpCovered = (Time.time - lerpStartTime) * speed;
        float fractionOfJourney = lerpCovered / lerpDifference;

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, fractionOfJourney);

        if (fractionOfJourney >= 1)
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
        lerpStartTime = Time.time;
    }
}
