using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private Shockwave shockwave;
    [SerializeField] private float abilityCD = 5f;
    private float timeSinceLastAbility = 0;
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if (anim == null)
        {
            throw new System.Exception("There is no Animator component.");
        }
    }

    private void Update()
    {
        timeSinceLastAbility += Time.deltaTime;
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
        if (timeSinceLastAbility > abilityCD)
        {
            anim.SetTrigger("UseAbility");
            Instantiate(shockwave, transform.position, transform.rotation, transform.parent);
            timeSinceLastAbility = 0;
            //Debug.Log("pog"); 
        }
    }
}
