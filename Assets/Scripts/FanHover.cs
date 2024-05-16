using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanHover : MonoBehaviour
{

    private Rigidbody rb;

    [SerializeField] float power = 10;
    [SerializeField] bool activated = true;

    void Start() {
        SetState(activated);
    }

   void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            rb = other.GetComponent<Rigidbody>();
        }
    }

    public void SetState(bool state) {
        activated = state;

        //A bit messy but only meant for debugging purposes
        gameObject.GetComponent<MeshRenderer>().enabled = state;
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            rb = null;
        }
    }

    void FixedUpdate() {
        if (rb != null && activated) {
            rb.velocity += gameObject.transform.up * power;
        }
    }
}
