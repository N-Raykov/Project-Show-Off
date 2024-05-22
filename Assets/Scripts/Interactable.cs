using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject interactionPromptPrefab;
    [SerializeField] PlayerInputReader reader;
    protected GameObject interactionPrompt;

    private void OnEnable()
    {
        reader.interactEventPerformed += PerformInteraction;
    }

    private void OnDisable()
    {
        reader.interactEventPerformed -= PerformInteraction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPrompt = Instantiate(interactionPromptPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(interactionPrompt);
        }
    }

    private void Update()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.transform.position = transform.position + Vector3.up * 2f;

            interactionPrompt.transform.rotation = Camera.main.transform.rotation;
        }
    }

    protected virtual void PerformInteraction()
    {
        if (interactionPrompt == null)
        {
            return;
        }
        Debug.Log("Interaction performed!");
    }
}
