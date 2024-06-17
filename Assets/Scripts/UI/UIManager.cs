using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image abilityImage;

    public static UIManager instance;

    public void AbilityUsed(float abilityCD)
    {
        StartCoroutine(AbilityCDInitiated(abilityCD));
    }

    public void ChangeTMProText(GameObject textObject, string message, ActionType actionType)
    {
        SetTextToTextBox promptText = textObject.GetComponent<SetTextToTextBox>();
        promptText.actionType = actionType;
        promptText.message = message;
    }

    private void Awake()
    {
        CreateSingleton();
    }

    private void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AbilityCDInitiated(float duration)
    {
        abilityImage.fillAmount = 0f;
        for (int i = 0; i < 100; i++)
        {
            abilityImage.fillAmount += 0.01f;
            yield return new WaitForSeconds(duration / 100);
        }
    }
}
