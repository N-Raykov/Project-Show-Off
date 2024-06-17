using UnityEngine;

public class Interactable : Prompt
{
    protected GameObject interactionPrompt;

    private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            interactionPrompt = Instantiate(prompt, transform.position + Vector3.up * 6f, Quaternion.identity);
            UIManager.instance.ChangeTMProText(interactionPrompt, message, actionType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Destroy(interactionPrompt);
        }
    }

    private void Update()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.transform.position = transform.position + Vector3.up * 6f;
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
