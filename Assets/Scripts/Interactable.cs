using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject interactionPromptPrefab;
    private GameObject interactionPrompt;

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

            if (Input.GetKeyDown(KeyCode.E))
            {
                PerformInteraction();
            }
        }
    }

    private void PerformInteraction()
    {
        Debug.Log("Interaction performed!");
    }
}
