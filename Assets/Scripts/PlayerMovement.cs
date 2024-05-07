using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityScale;

    private CustomPlayerInput input;
    private Rigidbody rb;
    private Vector2 moveVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            throw new System.Exception("There is no Rigidbody2D component.");
        }

        input = new CustomPlayerInput();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += OnMovementPerformed;
        input.Player.Move.canceled += OnMovementCancelled;
        input.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Move.performed -= OnMovementPerformed;
        input.Player.Move.canceled -= OnMovementCancelled;
        input.Player.Jump.performed -= OnJumpPerformed;
    }

    private void FixedUpdate()
    {
        Vector3 movement = moveVector * movementSpeed * Time.fixedDeltaTime;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.y);
    }

    private void OnMovementPerformed(InputAction.CallbackContext pValue)
    {
        moveVector = pValue.ReadValue<Vector2>();
        //Debug.Log("START MOVEMENT: " + moveVector.ToString());
    }

    private void OnMovementCancelled(InputAction.CallbackContext pValue)
    {
        moveVector = Vector2.zero;
        //Debug.Log("STOP MOVEMENT");
    }

    private void OnJumpPerformed(InputAction.CallbackContext pValue)
    {
        rb.AddForce(new Vector3(0, jumpForce, 0));
        Debug.Log("JUMP: " + jumpForce);
    }
}
