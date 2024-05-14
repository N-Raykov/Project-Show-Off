using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 100f;
    [SerializeField] private float jumpForceInitial = 10f;
    [SerializeField] private float jumpForceContinous = 1f;
    [SerializeField] private float maxJumpTime = 0.3f;
    [SerializeField] private float gravity = 40f;
    [SerializeField] private float gravityFallModifier = 4f;
    [SerializeField] private float groundDrag = 0.03f;
    [SerializeField] private float airDrag = 0.03f;

    private CustomPlayerInput input;
    private Rigidbody rb;
    private Vector2 moveVector;
    private float distToGround;
    private bool isGrounded;
    private float jumpTime;
    private bool isJumping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            throw new System.Exception("There is no Rigidbody component.");
        }

        input = new CustomPlayerInput();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += OnMovementPerformed;
        input.Player.Move.canceled += OnMovementCancelled;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Move.performed -= OnMovementPerformed;
        input.Player.Move.canceled -= OnMovementCancelled;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCancelled;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();

        HandleMovement();
        HandleJumping();
        HandleGravity();
        HandleDrag();
    }

    private void HandleMovement()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.forward);

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

        //camera forward and right vectors:
        Vector3 right = Camera.main.transform.right;
        Vector3 forward = Camera.main.transform.forward;

        //project forward and right vectors on the horizontal plane (y = 0)
        right.y = 0f;
        forward.y = 0f;
        right.Normalize();
        forward.Normalize();

        Vector3 desiredMoveDirection = right * moveVector.x + forward * moveVector.y;

        moveVector = new Vector2(desiredMoveDirection.x, desiredMoveDirection.z);

        //float delta = Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        //moveVector = new Vector2(
        //  moveVector.x * Mathf.Cos(delta) - moveVector.y * Mathf.Sin(delta),
        //moveVector.x * Mathf.Sin(delta) + moveVector.y * Mathf.Cos(delta));

        //Vector2 forwardRelative = moveVector.x * Camera.main.transform.right;
        //Vector2 rightRelative = moveVector.y * Camera.main.transform.up;
        //moveVector = forwardRelative + rightRelative;
        //Debug.Log("START MOVEMENT: " + moveVector.ToString());
    }

    private void OnMovementCancelled(InputAction.CallbackContext pValue)
    {
        moveVector = Vector2.zero;
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

    private bool IsGrounded()
    {
      return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
