using UnityEngine;
using Cinemachine;

public class PlayCutScene : Interactable
{
    [Space(10)]

    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private Animator animator;
    [SerializeField] private int priority;
    [SerializeField] private string triggerName;

    protected override void PerformInteraction()
    {
        if (interactionPrompt == null || debounce)
        {
            return;
        }

        if (vCam.gameObject.activeInHierarchy == false)
        {
            vCam.gameObject.SetActive(true);
        }

        EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Interact, transform.position));
        debounce = true;
        vCam.m_Priority = priority;
        animator.SetTrigger(triggerName);
        Destroy(interactionPrompt);

        reader.SetEnabledActionMap(false, false, false); Debug.Log("huh3");
    }
}
