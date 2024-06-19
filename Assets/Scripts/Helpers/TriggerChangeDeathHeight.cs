using UnityEngine;

public class TriggerChangeDeathHeight : MonoBehaviour
{
    [SerializeField] private float newRespawnHeight = 0f;

    private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {
            other.GetComponent<PlayerRespawn>().respawnHeight = newRespawnHeight;
        }
    }
}
