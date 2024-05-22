using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayCutScene : Interactable
{
    [Space(10)]

    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] int priority;
    [SerializeField] Animator animator;
    [SerializeField] string triggerName;
    protected override void PerformInteraction()
    {
        if (interactionPrompt == null)
        {
            return;
        }

        if (vCam.gameObject.activeInHierarchy == false)
        {
            vCam.gameObject.SetActive(true);
        }
        vCam.m_Priority = priority;
        animator.SetTrigger(triggerName);
    }
}
