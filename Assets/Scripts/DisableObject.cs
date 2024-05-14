using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    [SerializeField] GameObject[] targets;

    void DisableTargets()
    {
        foreach(GameObject target in targets)
        {
            target.SetActive(false);
        }
    }
}
