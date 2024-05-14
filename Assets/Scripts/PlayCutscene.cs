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
        vCam.m_Priority = priority;
        animator.SetTrigger(triggerName);
    }
}
