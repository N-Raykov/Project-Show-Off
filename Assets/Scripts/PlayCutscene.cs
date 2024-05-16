using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayCutScene : Interactable
{
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
        Debug.Log("what");
        if (vCam.gameObject.activeInHierarchy == false)
        {
            vCam.gameObject.SetActive(true);
        }
        vCam.m_Priority = priority;
        animator.SetTrigger(triggerName);
    }
}
