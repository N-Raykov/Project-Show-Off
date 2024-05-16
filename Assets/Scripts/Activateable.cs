using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activateable : MonoBehaviour
{

    public UnityEvent OnActivated;
    [SerializeField] bool activated = false;

    public void Activate()
    {
        if(activated == false)
        {
            Debug.Log("poggers");
            activated = true;
            OnActivated?.Invoke();
        }
    }
}
