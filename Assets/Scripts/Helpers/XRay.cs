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
    //change the array to a list and iterate through each material on the obstacle creating a *NEW* material for it from the renderer
    //instead of making the array = the renderer.materials
    private Dictionary<Transform, Material[]> originalMaterials;
    private Dictionary<Transform, Coroutine> fadeCoroutines;
    void Start()
    {
        oldHitsNumber = 0;
        ignoredLayers = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("IgnoredByXRay"));
        originalMaterials = new Dictionary<Transform, Material[]>();
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
                    RestoreOriginalMaterials(obstructions[i]);
                }
            }

            obstructions = new Transform[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                Transform obstruction = hits[i].transform;
                obstructions[i] = obstruction;
                if (!originalMaterials.ContainsKey(obstruction) && obstruction.GetComponent<MeshRenderer>() != null)
                {
                    originalMaterials[obstruction] = (Material[])obstruction.GetComponent<MeshRenderer>().materials.Clone();
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
                    RestoreOriginalMaterials(obstructions[i]);
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

    private void RestoreOriginalMaterials(Transform obstruction)
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
                fadeCoroutines[obstruction] = StartCoroutine(FadeToAlpha(renderer, 1f, originalMaterials[obstruction]));
            }
            originalMaterials.Remove(obstruction);
        }
    }

    private IEnumerator FadeToAlpha(MeshRenderer renderer, float targetAlpha, Material[] restoreMaterials = null)
    {
        Material[] materials = (Material[])renderer.materials.Clone();
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
        }

        // Set render mode to opaque if the target alpha is 1
        if (targetAlpha == 1f)
        {
            foreach (Material material in materials)
            {
                ChangeMaterialRenderMode(material, RenderMode.Opaque);
            }

            // Restore the materials if provided
            if (restoreMaterials != null)
            {
                renderer.materials = restoreMaterials;
            }
        }
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
