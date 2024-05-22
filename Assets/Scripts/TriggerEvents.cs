using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public UnityEvent enterEvent;
    public UnityEvent exitEvent;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            enterEvent?.Invoke();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            exitEvent?.Invoke();
        }
    }
}
