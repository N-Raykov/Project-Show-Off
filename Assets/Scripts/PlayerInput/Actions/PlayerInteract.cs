using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;

    private void OnEnable()
    {
        reader.interactEventPerformed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        reader.interactEventPerformed -= OnInteractPerformed;
    }

    private void OnInteractPerformed()
    {
    }
}
