using UnityEngine;
using System;
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
        EventBus<CollectibleGathered>.OnEvent += OnCollectibleGathered;
    }

    private void OnDisable()
    {
        EventBus<CollectibleGathered>.OnEvent -= OnCollectibleGathered;
    }

    private void Start()
    {
        collectibleTMPro = GetComponent<TextMeshProUGUI>();
        if (collectibleTMPro == null)
        {
            throw new Exception("There is no TextMeshProUGUI component.");
        }
        collectibleTMPro.SetText(text + collectibleCount);

        originalScale = transform.localScale;
        targetScale = originalScale * scaleIncrease;
        lerpDifference = targetScale.magnitude - originalScale.magnitude;
    }

    private void Update()
    {
        if(isLerping)
            LerpScale();
    }

    //When you collect a collectible it makes the icon lerp to being slightly bigger and then back to the original size
    private void OnCollectibleGathered(CollectibleGathered pCollectibleGathered)
    {
        collectibleCount += pCollectibleGathered.value;
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
        }
    }

    private void StartLerp()
    {
        transform.localScale = originalScale;
        isLerping = true;
        lerpStartTime = Time.time;
    }
}
