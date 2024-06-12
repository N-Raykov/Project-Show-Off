using UnityEngine;
using AYellowpaper.SerializedCollections;
public class PlayerRespawn : AbstractPlayerAction
{
    public float respawnHeight = 10f;

    public Vector3 activeRespawnPoint;

    private void Start()
    {
        activeRespawnPoint = transform.position;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        HandleRespawn();
    }

    private void HandleRespawn()
    {
        if(transform.position.y < respawnHeight)
        {
            transform.position = activeRespawnPoint;
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
}
