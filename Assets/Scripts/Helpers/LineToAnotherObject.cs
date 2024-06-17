using UnityEngine;

[ExecuteAlways]
public class LineToAnotherObject : MonoBehaviour
{
    public GameObject objTarget;

    private void Update()
    {
        Debug.DrawLine(transform.position, objTarget.transform.position, Color.red);
    }
}
