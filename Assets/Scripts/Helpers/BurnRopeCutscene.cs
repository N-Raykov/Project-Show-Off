using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BurnRopeCutscene : MonoBehaviour
{
    [SerializeField]
    Material material;

    [SerializeField]
    float startBlend;

    [SerializeField]
    float targetBlend;

    [SerializeField]
    float lerpDuration = 1.0f;

    private void Start()
    {
        material.SetFloat("_DissolveProgress", 0f);
    }

    public void PlayLerp()
    {
        material.DOFloat(targetBlend, "_DissolveProgress", lerpDuration).SetEase(Ease.OutSine);
    }
}
