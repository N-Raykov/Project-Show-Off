using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class XRay : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Range(0, 1)]
    [SerializeField] private float alpha = 0.5f;

    [SerializeField] private float fadeDuration = 1f;

    [Header("Don't touch")]
    [SerializeField] private Transform[] obstructions;
    private LayerMask ignoredLayers;
    private int oldHitsNumber;

    private Dictionary<Transform, Material> originalMaterials;
    private Dictionary<Transform, Coroutine> fadeCoroutines;
    void Start()
    {
        oldHitsNumber = 0;
        ignoredLayers = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("IgnoredByXRay"));
        originalMaterials = new Dictionary<Transform, Material>();
        fadeCoroutines = new Dictionary<Transform, Coroutine>();
    }

    private void LateUpdate()
    {
        ViewObstructed();
    }

    //Gets the objects that are obstructing the view
    void ViewObstructed()
    {
        float characterDistance = Vector3.Distance(transform.position, player.transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, player.position - transform.position, characterDistance, ~ignoredLayers);
        if (hits.Length > 0)
        {
            int newHits = hits.Length - oldHitsNumber;

            if (obstructions != null && obstructions.Length > 0 && newHits < 0)
            {
                for (int i = 0; i < obstructions.Length; i++)
                {
                    RestoreOriginalAlpha(obstructions[i]);
                }
            }

            obstructions = new Transform[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                Transform obstruction = hits[i].transform;
                obstructions[i] = obstruction;
                if (!originalMaterials.ContainsKey(obstruction) && obstruction.GetComponent<MeshRenderer>() != null)
                {
                    originalMaterials[obstruction] = new Material(obstruction.GetComponent<MeshRenderer>().material);
                }
                MakeTransparent(obstruction);
            }

            oldHitsNumber = hits.Length;
        }
        else
        {
            if (obstructions != null && obstructions.Length > 0)
            {
                for (int i = 0; i < obstructions.Length; i++)
                {
                    RestoreOriginalAlpha(obstructions[i]);
                }
                oldHitsNumber = 0;
                obstructions = null;
            }
        }
    }

    private void MakeTransparent(Transform obstruction)
    {
        MeshRenderer renderer = obstruction.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            if (fadeCoroutines.ContainsKey(obstruction))
            {
                StopCoroutine(fadeCoroutines[obstruction]);
            }
            fadeCoroutines[obstruction] = StartCoroutine(FadeToAlpha(renderer, alpha));
        }
    }

    private void RestoreOriginalAlpha(Transform obstruction)
    {
        if (originalMaterials.ContainsKey(obstruction))
        {
            MeshRenderer renderer = obstruction.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                if (fadeCoroutines.ContainsKey(obstruction))
                {
                    StopCoroutine(fadeCoroutines[obstruction]);
                }
                fadeCoroutines[obstruction] = StartCoroutine(FadeToAlpha(renderer, originalMaterials[obstruction].color.a, originalMaterials[obstruction].GetFloat("_Surface")));
            }
        }
    }

    private IEnumerator FadeToAlpha(MeshRenderer renderer, float targetAlpha, float originalRenderMode = 0)
    {
        Material[] materials = renderer.materials;
        float startAlpha = materials[0].color.a;
        float elapsedTime = 0f;

        // Set render mode at the beginning of the fade
        if (targetAlpha < 1f)
        {
            foreach (Material material in materials)
            {
                ChangeMaterialRenderMode(material, RenderMode.Transparent);
            }
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            foreach (Material material in materials)
            {
                Color color = material.color;
                color.a = newAlpha;
                material.color = color;
            }
            yield return null;
        }

        foreach (Material material in materials)
        {
            Color color = material.color;
            color.a = targetAlpha;
            material.color = color;
            if (originalRenderMode == 0)
            {
                ChangeMaterialRenderMode(material, RenderMode.Opaque);
            }
            else
            {
                ChangeMaterialRenderMode(material, RenderMode.Transparent);
            }
        }

        originalMaterials.Remove(renderer.gameObject.transform);
    }

    private void ChangeMaterialRenderMode(Material material, RenderMode blendMode)
    {
        switch (blendMode)
        {
            case RenderMode.Opaque:
                material.SetFloat("_Surface", 0);
                material.SetInt("_SrcBlend", (int)BlendMode.One);
                material.SetInt("_DstBlend", (int)BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderMode.Transparent:
                material.SetFloat("_Surface", 1);
                material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }

    private enum RenderMode
    {
        Opaque,
        Transparent
    }
}
