using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanHover : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField] float power = 10;

   void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            rb = other.GetComponent<Rigidbody>();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            rb = null;
        }
    }

    void FixedUpdate() {
        if (rb != null) {
            rb.velocity += gameObject.transform.up * power;
        }
    }
}
