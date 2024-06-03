using UnityEngine;
using System;

public class PlayerJump : AbstractPlayerAction
{
    [Header("Jumping")]
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private float jumpForceInitial = 16f;
    [SerializeField] private float jumpForceContinous = 50f;
    [SerializeField] private float maxJumpTime = 0.3f;
    [SerializeField] private float gravity = 80f;
    [SerializeField] private float gravityFallModifier = 1f;
    [SerializeField] GameObject rangeIndicatorPrefab;

    private PlayerMovement playerMovement;
    [Range(10, 100)] private int linePoints = 25;
    private float terminalRunningSpeed;
    private bool isJumping;
    private float jumpTime = 0;

    private float t;

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

        terminalRunningSpeed = 0;
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            for (int i = 0; i <= 1000; i++)
            {

                terminalRunningSpeed += playerMovement.movementSpeed * Time.fixedDeltaTime;
                terminalRunningSpeed *= Mathf.Clamp01(1 - playerMovement.groundDrag * Time.fixedDeltaTime);
            }
        }

        JumpRangeIndicator jumpRangeIndicator = GetComponentInChildren<JumpRangeIndicator>();
        if (jumpRangeIndicator != null)
        {
            jumpRangeIndicator.Initialize(playerMovement, distToBottomOfSprite, terminalRunningSpeed, jumpForceContinous, linePoints, gravityFallModifier, jumpForceInitial, gravity, maxJumpTime);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        HandleJumping();

        if (!isGrounded)
        {
            HandleGravity();
        }
    }

    private void OnJumpPerformed()
    {
        if (!isGrounded)
            return;

        isJumping = true;

        rb.velocity = new Vector3(
            rb.velocity.x,
            rb.velocity.y + jumpForceInitial,
            rb.velocity.z);

        EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.PlayerJump));

        InitializeRangeIndicatorPrefab();

        //Debug.Log("START JUMP - jumpForceInitial: " + jumpForceInitial);
        //Debug.Log("Velocity at jump start: " + rb.velocity);
    }

    private void OnJumpCancelled()
    {
        isJumping = false;
        //Debug.Log("STOP JUMP");
    }

    private void HandleJumping()
    {
        if (!isJumping || jumpTime >= maxJumpTime)
        {
            isJumping = false;
            jumpTime = 0;
            return;
        }

        //t += jumpForceContinous * Time.fixedDeltaTime;

        jumpTime += Time.fixedDeltaTime;

        rb.velocity = new Vector3(
            rb.velocity.x,
            rb.velocity.y + jumpForceContinous * Time.fixedDeltaTime,
            rb.velocity.z);        
    }

    private void HandleGravity()
    {
        if (anim != null)
        {
            anim.SetFloat("JumpingBlend", rb.velocity.y <= 0 ? 1 : 0);
        }

        float currentGravity = gravity * (rb.velocity.y < 0 ? gravityFallModifier : 1);
        rb.velocity = new Vector3(
            rb.velocity.x,
            rb.velocity.y - currentGravity * Time.fixedDeltaTime,
            rb.velocity.z); //Debug.Log("1:" + (-currentGravity * Time.fixedDeltaTime));
    }

    private void InitializeRangeIndicatorPrefab()
    {
        if (rangeIndicatorPrefab == null)
            return;

        GameObject prefab = Instantiate(rangeIndicatorPrefab);
        prefab.transform.position = new Vector3(transform.position.x, transform.position.y - distToBottomOfSprite, transform.position.z);
        JumpRangeIndicator jumpRangeIndicator = prefab.GetComponent<JumpRangeIndicator>();
        if (jumpRangeIndicator != null)
        {
            jumpRangeIndicator.Initialize(playerMovement, distToBottomOfSprite, terminalRunningSpeed, jumpForceContinous, linePoints, gravityFallModifier, jumpForceInitial, gravity, maxJumpTime);
        }
    }
}
