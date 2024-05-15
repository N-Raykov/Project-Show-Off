using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if (anim == null)
        {
            throw new System.Exception("There is no Animator component.");
        }
    }

    private void OnEnable()
    {
        reader.jumpEventPerformed += OnAbilityPerformed;
    }

    private void OnDisable()
    {
        reader.jumpEventPerformed -= OnAbilityPerformed;
    }

    private void OnAbilityPerformed()
    {
        anim.SetTrigger("UseAbility");
        //Debug.Log("pog"); 
    }
}
