using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Prompt
{
    protected GameObject interactionPrompt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPrompt = Instantiate(prompt, transform.position + Vector3.up * 8f, Quaternion.identity);
            UIManager.instance.ChangeTMProText(interactionPrompt, message, actionType);
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
            interactionPrompt.transform.position = transform.position + Vector3.up * 8f;

            interactionPrompt.transform.rotation = Camera.main.transform.rotation;
        }
    }

    protected override void PerformInteraction()
    {
        if (interactionPrompt == null)
        {
            return;
        }
    }
}
