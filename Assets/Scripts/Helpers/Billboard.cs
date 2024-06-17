using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z), Vector3.up);
    }
}
