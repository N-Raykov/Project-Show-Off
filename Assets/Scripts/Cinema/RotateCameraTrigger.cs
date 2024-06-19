using Cinemachine;
using UnityEngine;

public class RotateCameraTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private int priority;
    [SerializeField] private bool targetState;

    private string playerTag = "Player";

    //Changes the active virtual camera
    void OnTriggerEnter(Collider other) {
        // Debug.Log("Collided with: " + other.gameObject);
        if (other.gameObject.tag == playerTag) {
            virtualCamera.gameObject.SetActive(targetState);
            virtualCamera.m_Priority = priority;
        }
    }
}
