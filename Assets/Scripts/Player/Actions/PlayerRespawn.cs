using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerRespawn : AbstractPlayerAction
{
    public Vector3 activeRespawnPoint;
    public float respawnHeight = 10f;

    [SerializeField] Image panelFade;

    private bool debounce = false;

    private void Start()
    {
        activeRespawnPoint = transform.position;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        HandleRespawn();
    }

    IEnumerator FadeTransition() {
        panelFade.DOFade(1.0f, 0.25f);
        yield return new WaitForSeconds(0.5f);
        transform.position = activeRespawnPoint;
        rb.velocity = new Vector3(0, 0, 0);
        panelFade.DOFade(0f, 0.25f);
        debounce = false;
    }

    private void HandleRespawn()
    {
        if(transform.position.y < respawnHeight && debounce == false)
        {
            Respawn();
        }
    }
    
    public void Respawn()
    {
        debounce = true;
        StartCoroutine(FadeTransition());
    }
}
