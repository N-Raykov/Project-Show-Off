using Cinemachine;
using UnityEngine;

public class SetCameraState : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private int priority;
    [SerializeField] private bool targetState;

    private void UpdateState()
    {
        virtualCamera.gameObject.SetActive(targetState);
        virtualCamera.m_Priority = priority;
    }
}
