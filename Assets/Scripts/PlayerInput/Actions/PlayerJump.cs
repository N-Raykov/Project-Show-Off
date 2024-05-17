using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : AbstractPlayerAction
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private float jumpForceInitial = 10f;
    [SerializeField] private float jumpForceContinous = 1f;
    [SerializeField] private float maxJumpTime = 0.3f;
    [SerializeField] private float gravity = 40f;
    [SerializeField] private float gravityFallModifier = 4f;
    [SerializeField] private float airDrag = 0.075f;

    private bool isJumping;
    private float jumpTime;

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
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();

        HandleJumping();
        HandleGravity();

        if(!isGrounded)
            HandleAirDrag();
    }

    private void OnJumpPerformed()
    {
        //if (!isGrounded)
            //return;

        isJumping = true;

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForceInitial, rb.velocity.z);

        //Debug.Log("START JUMP: " + jumpForce);
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

        jumpTime += Time.fixedDeltaTime;

        rb.velocity = new Vector3(
            rb.velocity.x,
            rb.velocity.y + jumpForceContinous,
            rb.velocity.z);
    }

    private void HandleGravity()
    {
        anim.SetFloat("JumpingBlend", (rb.velocity.y <= 0 ? 1 : 0));

        float currentGravity = gravity * Time.fixedDeltaTime * (rb.velocity.y <= 0 ? gravityFallModifier : 1);
        rb.velocity = new Vector3(
            rb.velocity.x,
            rb.velocity.y - currentGravity,
            rb.velocity.z);
    }

    private void HandleAirDrag()
    {
        rb.velocity = new Vector3(
            rb.velocity.x * (1 - airDrag),
            rb.velocity.y,
            rb.velocity.z * (1 - airDrag));
    }
}
