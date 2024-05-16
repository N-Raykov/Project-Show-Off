using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private Shockwave shockwave;
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
        reader.abilityEventPerformed += OnAbilityPerformed;
    }

    private void OnDisable()
    {
        reader.abilityEventPerformed -= OnAbilityPerformed;
    }

    private void OnAbilityPerformed()
    {
        anim.SetTrigger("UseAbility");
        Instantiate(shockwave, transform.position, transform.rotation, transform.parent);
        //Debug.Log("pog"); 
    }
}
