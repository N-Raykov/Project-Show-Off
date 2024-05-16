using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private float jumpForceInitial = 10f;
    [SerializeField] private float jumpForceContinous = 1f;
    [SerializeField] private float maxJumpTime = 0.3f;
    [SerializeField] private float gravity = 40f;
    [SerializeField] private float gravityFallModifier = 4f;
    [SerializeField] private float airDrag = 0.03f;

    private bool isJumping;
    private float jumpTime;

    private Animator anim;
    private Rigidbody rb;
    private bool isGrounded;
    private float distToBottomOfSprite;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            throw new System.Exception("There is no Rigidbody component.");
        }

        anim = GetComponentInChildren<Animator>();
        if (anim == null)
        {
            throw new System.Exception("There is no Animator component.");
        }

        distToBottomOfSprite = GetComponent<Collider>().bounds.extents.y;
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
        if (!isGrounded)
            return;

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

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToBottomOfSprite + 0.1f, ~0, QueryTriggerInteraction.Ignore);
    }
}
