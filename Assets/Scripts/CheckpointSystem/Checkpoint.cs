using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject respawnPoint;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, respawnPoint.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerRespawn>().activeRespawnPoint = respawnPoint.transform.position;
        }
    }
}