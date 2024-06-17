using UnityEngine;

public class DisableObject : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;

    private void DisableTargets()
    {
        foreach(GameObject target in targets)
        {
            target.SetActive(false);
        }
    }
}
