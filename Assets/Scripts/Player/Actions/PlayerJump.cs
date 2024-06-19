using UnityEngine;
using System;

public struct JumpIndicatorDataPlayerJump
{
    public float distToBottomOfSprite;
    public float terminalRunningSpeed;
    public float jumpForceContinous;
    public float jumpForceInitial;
    public float gravity;
    public float gravityFallModifier;
    public float maxJumpTime;

    public JumpIndicatorDataPlayerJump(float pDistToBottomOfSprite, float pTerminalRunningSpeed, float pJumpForceContinous, float pJumpForceInitial, float pGravity, float pGravityFallModifier, float pMaxJumpTime)
    {
        distToBottomOfSprite = pDistToBottomOfSprite;
        terminalRunningSpeed = pTerminalRunningSpeed;
        jumpForceContinous = pJumpForceContinous;
        jumpForceInitial = pJumpForceInitial;
        gravity = pGravity;
        gravityFallModifier = pGravityFallModifier;
        maxJumpTime = pMaxJumpTime;
    }
}

public class PlayerJump : AbstractPlayerAction
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private float jumpForceInitial = 16f;
    [SerializeField] private float jumpForceContinous = 50f;
    [SerializeField] private ParticleSystem jumpingParticles;
    [SerializeField] private ParticleSystem landingParticles;
    [SerializeField] private float maxJumpTime = 0.3f;
    [SerializeField] private float gravity = 80f;
    [SerializeField] private float gravityFallModifier = 1f;
    [SerializeField] GameObject rangeIndicatorPrefab;

    private PlayerMovement playerMovement;
    private bool isJumping;
    private float jumpTime = 0;
    private bool _isGrounded;
    private string jumpingBlendParamName = "JumpingBlend";

    // private bool _isGrounded;

    public JumpIndicatorDataPlayerJump GetJumpIndicatorData()
    {
        return new JumpIndicatorDataPlayerJump(distToBottomOfSprite, CalculateTerminalRunningSpeed(), jumpForceContinous, jumpForceInitial, gravity, gravityFallModifier, maxJumpTime);
    }

    private void OnEnable()
    {
        reader.jumpEventPerformed += OnJumpPerformed;
        reader.jumpEventCancelled += OnJumpCancelled;
    }

    private void OnDisable()
    {
        reader.jumpEventPerformed -= OnJumpPerformed;
        reader.jumpEventCancelled -= OnJumpCancelled;
    }

    new private void Awake()
    {
        base.Awake();

        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            throw new Exception("There is a missing PlayerMovement component.");
        }

        JumpRangeIndicator jumpRangeIndicator = GetComponentInChildren<JumpRangeIndicator>();
        if (jumpRangeIndicator != null)
        {
            jumpRangeIndicator.Initialize(playerMovement, this);
        }
    }

    private float CalculateTerminalRunningSpeed()
    {
        JumpIndicatorDataPlayerMovement jumpIndicatorDataPlayerMovement = playerMovement.GetJumpIndicatorData();
        float terminalRunningSpeed = ((jumpIndicatorDataPlayerMovement.movementSpeed / jumpIndicatorDataPlayerMovement.groundDrag) - Time.fixedDeltaTime * jumpIndicatorDataPlayerMovement.movementSpeed) / rb.mass; //Debug.Log("terminalRunningSpeed: " + terminalRunningSpeed);
        return terminalRunningSpeed;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        OnPlayerGrounded = isGrounded;

        HandleJumping();

        if (!isGrounded)
        {
            HandleGravity();
        }
    }

    //Plays particle effects when the player becomes grounded
    private bool OnPlayerGrounded
    {
        set
        {
            if(_isGrounded == false && value == true)
            {
                //Player lands
                landingParticles.Play();
            }
            _isGrounded = value;
        }
    }

    private void OnJumpPerformed()
    {
        if (!isGrounded)
            return;

        isJumping = true; //Debug.Log("START JUMP: " + jumpForce);

        rb.velocity = new Vector3(
            rb.velocity.x,
            rb.velocity.y + jumpForceInitial,
            rb.velocity.z); //Debug.Log("Velocity at jump start: " + rb.velocity);

        EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.PlayerJump, transform.position));

        InitializeRangeIndicatorPrefab();        
        
        jumpingParticles.Play();

        if (anim != null)
        {
            anim.SetFloat(jumpingBlendParamName, 0);
        }
    }

    private void OnJumpCancelled()
    {
        isJumping = false; //Debug.Log("STOP JUMP");
    }

    //Makes the player jump
    private void HandleJumping()
    {
        if (!isJumping || jumpTime >= maxJumpTime)
        {
            isJumping = false;
            jumpTime = 0;
            return;
        }

        jumpTime += Time.fixedDeltaTime;

        rb.velocity = new Vector3(
            rb.velocity.x,
            rb.velocity.y + jumpForceContinous * Time.fixedDeltaTime,
            rb.velocity.z);        
    }

    private void HandleGravity()
    {
        if (anim != null && rb.velocity.y <= 0)
        {
            anim.SetFloat(jumpingBlendParamName, 1);
        }

        float currentGravity = gravity * (rb.velocity.y < 0 ? gravityFallModifier : 1);
        rb.velocity = new Vector3(
            rb.velocity.x,
            rb.velocity.y - currentGravity * Time.fixedDeltaTime,
            rb.velocity.z); //Debug.Log("1:" + (-currentGravity * Time.fixedDeltaTime)); //Debug.Log("-currentGravity: " + (-currentGravity));

    }

    //Creates the jump indicator
    private void InitializeRangeIndicatorPrefab()
    {
        if (rangeIndicatorPrefab == null)
            return;

        GameObject prefab = Instantiate(rangeIndicatorPrefab);
        prefab.transform.position = new Vector3(transform.position.x, transform.position.y - distToBottomOfSprite, transform.position.z);
        JumpRangeIndicator jumpRangeIndicator = prefab.GetComponent<JumpRangeIndicator>();
        if (jumpRangeIndicator != null)
        {
            jumpRangeIndicator.Initialize(playerMovement, this);
        }
    }
}
