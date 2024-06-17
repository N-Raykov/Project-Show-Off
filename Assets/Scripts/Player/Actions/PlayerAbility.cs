using UnityEngine;

public class PlayerAbility : AbstractPlayerAction
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private ParticleSystem abilityParticles;
    [SerializeField] private Shockwave shockwave;
    [SerializeField] private float abilityCD = 5f;

    private float timeSinceLastAbility = 100;
    private string useAbilityParamName = "UseAbility";

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
            if (anim != null)
            {
                anim.SetTrigger(useAbilityParamName);
            }
            abilityParticles.Play();
            Instantiate(shockwave, transform.position, transform.rotation, transform.parent);
            timeSinceLastAbility = 0;
            EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Ability, transform.position));
            UIManager.instance.AbilityUsed(abilityCD);
        }
    }
}
