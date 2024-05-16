using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovementOld : MonoBehaviour
{
    [SerializeField] Shockwave shockwave;

    [SerializeField] private float movementSpeed = 100f;
    [SerializeField] private float jumpForceInitial = 10f;
    [SerializeField] private float jumpForceContinous = 1f;
    [SerializeField] private float maxJumpTime = 0.3f;
    [SerializeField] private float gravity = 40f;
    [SerializeField] private float gravityFallModifier = 4f;
    [SerializeField] private float groundDrag = 0.03f;
    [SerializeField] private float airDrag = 0.03f;

    [SerializeField] private Animator anim;


    private CustomPlayerInput input;
    private Rigidbody rb;
    private Camera mainCamera;
    private Vector2 moveVector;
    private float distToBottomOfSprite;
    private bool isGrounded;
    private float jumpTime;
    private bool isJumping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // anim = GetComponentInChildren<Animator>();
        if (rb == null)
        {
            throw new System.Exception("There is no Rigidbody component.");
        }

        input = new CustomPlayerInput();
        distToBottomOfSprite = GetComponent<Collider>().bounds.extents.y;
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += OnMovementPerformed;
        input.Player.Move.canceled += OnMovementCancelled;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCancelled;
        input.Player.Ability.performed += OnAbilityPerformed;
        input.Player.Ability.canceled += OnAbilityCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Move.performed -= OnMovementPerformed;
        input.Player.Move.canceled -= OnMovementCancelled;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCancelled;
        input.Player.Ability.performed -= OnAbilityPerformed;
        input.Player.Ability.canceled -= OnAbilityCancelled;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        anim.SetBool("isGrounded", IsGrounded());

        HandleMovement();
        HandleJumping();
        HandleGravity();
        HandleDrag();
    }

    private void HandleMovement()
    {
        Vector3 currentMovement = moveVector * movementSpeed * Time.fixedDeltaTime;

        rb.velocity = new Vector3(
            rb.velocity.x + currentMovement.x,
            rb.velocity.y,
            rb.velocity.z + currentMovement.y);
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
        float currentGravity = gravity * Time.fixedDeltaTime * (rb.velocity.y <= 0 ? gravityFallModifier : 1);

        anim.SetFloat("JumpingBlend", (rb.velocity.y <= 0 ? 1 : 0));

        rb.velocity = new Vector3(
            rb.velocity.x,
            rb.velocity.y - currentGravity,
            rb.velocity.z);
    }

    private void HandleDrag()
    {
        float currentDrag = isGrounded ? groundDrag : airDrag;

        rb.velocity = new Vector3(
            rb.velocity.x * (1 - currentDrag),
            rb.velocity.y * (1 - currentDrag),
            rb.velocity.z * (1 - currentDrag));
    }

    private void OnMovementPerformed(InputAction.CallbackContext pValue)
    {
        moveVector = pValue.ReadValue<Vector2>();

        //project forward and right camera vectors on the horizontal plane (y = 0)
        Vector3 right = mainCamera.transform.right;
        Vector3 forward = mainCamera.transform.forward;
        right.y = 0f;
        forward.y = 0f;
        right.Normalize();
        forward.Normalize();

        Vector3 desiredMoveDirection = right * moveVector.x + forward * moveVector.y;
        moveVector = new Vector2(desiredMoveDirection.x, desiredMoveDirection.z);


        anim.SetFloat("MovementBlend", 1f);

        //Debug.Log("START MOVEMENT: " + moveVector.ToString());
    }

    private void OnMovementCancelled(InputAction.CallbackContext pValue)
    {
        moveVector = Vector2.zero;

        anim.SetFloat("MovementBlend", 0f);
        //Debug.Log("STOP MOVEMENT");
    }

    private void OnJumpPerformed(InputAction.CallbackContext pValue)
    {
        if (!isGrounded)
            return;

        isJumping = true;

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForceInitial, rb.velocity.z);

        //Debug.Log("START JUMP: " + jumpForce);
    }

    private void OnJumpCancelled(InputAction.CallbackContext pValue)
    {
        isJumping = false;

        //Debug.Log("STOP JUMP: " + jumpForce);
    }

    private void OnAbilityPerformed(InputAction.CallbackContext pValue)
    {
        anim.SetTrigger("UseAbility");
        Instantiate(shockwave, transform.position, transform.rotation, transform.parent);
        //Debug.Log("pog"); 
    }

    private void OnAbilityCancelled(InputAction.CallbackContext pValue)
    {
        //Debug.Log("no more pog");
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToBottomOfSprite + 0.1f, ~0, QueryTriggerInteraction.Ignore);
    }
}
