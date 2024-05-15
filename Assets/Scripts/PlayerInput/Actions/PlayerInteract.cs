using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;

    private void OnEnable()
    {
        reader.jumpEventPerformed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        reader.jumpEventPerformed -= OnInteractPerformed;
    }

    private void OnInteractPerformed()
    {
    }
}
