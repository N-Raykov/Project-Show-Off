using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChangeDeathHeight : MonoBehaviour
{
    [SerializeField] private float newRespawnHeight = 0f;

  void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.GetComponent<PlayerRespawn>().respawnHeight = newRespawnHeight;
        }
  }
}
