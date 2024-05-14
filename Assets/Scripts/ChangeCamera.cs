using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCamera : Interactable
{
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] int priority;

    protected override void PerformInteraction()
    {
        vCam.m_Priority = priority;
    }
}
