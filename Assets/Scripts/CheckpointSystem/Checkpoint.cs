using UnityEngine;

[ExecuteAlways]
public class Checkpoint : MonoBehaviour
{
    public GameObject respawnPoint;

    private void Awake()
    {
        CreateChildGameObject();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, respawnPoint.transform.position, Color.red);
    }

    private void CreateChildGameObject()
    {
        if (respawnPoint != null)
        {
            return;
        }

        respawnPoint = new GameObject("RespawnPoint");

        respawnPoint.transform.SetParent(this.transform);

        respawnPoint.transform.localPosition = Vector3.zero; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerRespawn>().activeRespawnPoint = respawnPoint.transform.position;
            Debug.Log("poggers");
        }
    }
}