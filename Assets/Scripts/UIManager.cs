using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image abilityImage;

    public static UIManager instance;

    private void Awake()
    {
        CreateSingleton();
    }

    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AbilityUsed(float abilityCD)
    {
        StartCoroutine(AbilityCDInitiated(abilityCD));
    }

    IEnumerator AbilityCDInitiated(float duration)
    {
        abilityImage.fillAmount = 0f;
        for (int i = 0; i < 100; i++)
        {
            abilityImage.fillAmount += 0.01f;
            yield return new WaitForSeconds(duration / 100);
        }
    }
}
