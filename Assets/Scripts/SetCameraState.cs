using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SetCameraState : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] int priority;
    [SerializeField] bool targetState;

    void OnTriggerEnter(Collider other) {
        // Debug.Log("Collided with: " + other.gameObject);
        if (other.gameObject.tag == "Player") {
            virtualCamera.gameObject.SetActive(targetState);
            virtualCamera.m_Priority = priority;
        }
    }
}
