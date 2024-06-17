using UnityEngine;

public class PlayerRespawn : AbstractPlayerAction
{
    public Vector3 activeRespawnPoint;
    public float respawnHeight = 10f;

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
