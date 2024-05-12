using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanHover : MonoBehaviour
{

    private Rigidbody rigidbody;
    [SerializeField] float power = 10;

   void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            rigidbody = other.GetComponent<Rigidbody>();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            rigidbody = null;
        }
    }

    void FixedUpdate() {
        if (rigidbody != null) {
            rigidbody.velocity += gameObject.transform.up * power;
        }
    }
}
