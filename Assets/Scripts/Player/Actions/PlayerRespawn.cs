using UnityEngine;

public class PlayerRespawn : AbstractPlayerAction
{
    public float respawnHeight = 10f;
    [SerializeField] private float respawnPointInterval = 0.5f;

    private Vector3 respawnPoint;
    private float currentTime;

    private void Start()
    {
        respawnPoint = transform.position;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        HandleRespawn();
        TryGetRespawnPoint();
    }

    private void HandleRespawn()
    {
        if(transform.position.y < respawnHeight)
        {
            transform.position = respawnPoint;
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    private void TryGetRespawnPoint()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= respawnPointInterval)
        {
            if (isGrounded)
                respawnPoint = transform.position;

            currentTime = 0;
        }
    }
}
