using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject respawnPoint;

    private string playerTag = "Player";

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, respawnPoint.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {
            other.GetComponent<PlayerRespawn>().activeRespawnPoint = respawnPoint.transform.position;
        }
    }
}