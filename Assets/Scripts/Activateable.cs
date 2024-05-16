using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activateable : MonoBehaviour
{
    [SerializeField] bool activated = false;
    public void Activate()
    {
        if(activated == false)
        {
            Debug.Log("poggers");
            activated = true;
        }
    }
}
