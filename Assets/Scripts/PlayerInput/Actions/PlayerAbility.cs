using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbility : AbstractPlayerAction
{
    [SerializeField] private PlayerInputReader reader;

    new private void Awake ()
    {
        base.Awake();
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
