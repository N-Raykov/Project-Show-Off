using UnityEngine;

public class PlayerMovement : AbstractPlayerAction
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private float movementSpeed = 100f;
    [SerializeField] private float groundDrag = 0.1f;

    private Camera mainCamera;
    private Vector2 moveVector;

    new private void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        reader.moveEventPerformed += OnMovementPerformed;
        reader.moveEventCancelled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        reader.moveEventPerformed -= OnMovementPerformed;
        reader.moveEventCancelled -= OnMovementCancelled;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        anim.SetBool("isGrounded", IsGrounded());

        HandleMovement();
         
        if(isGrounded)
            HandleGroundDrag();
    }

    private void HandleMovement()
    {
        Vector3 currentMovement = moveVector * movementSpeed * Time.fixedDeltaTime;

        rb.velocity = new Vector3(
            rb.velocity.x + currentMovement.x,
            rb.velocity.y,
            rb.velocity.z + currentMovement.y);
    }
    private void HandleGroundDrag()
    {
        rb.velocity = new Vector3(
            rb.velocity.x * (1 - groundDrag),
            rb.velocity.y,
            rb.velocity.z * (1 - groundDrag));
    }

    private void OnMovementPerformed(Vector2 pMoveVector)
    {
        anim.SetFloat("MovementBlend", 1f);

        //project forward and right camera vectors on the horizontal plane (y = 0)
        Vector3 right = mainCamera.transform.right;
        Vector3 forward = mainCamera.transform.forward;
        right.y = 0f;
        forward.y = 0f;
        right.Normalize();
        forward.Normalize();

        //Create move vector relative to camera rotation
        Vector3 desiredMoveDirection = right * pMoveVector.x + forward * pMoveVector.y;
        moveVector = new Vector2(desiredMoveDirection.x, desiredMoveDirection.z);

        //Debug.Log("START MOVEMENT: " + moveVector.ToString());
    }

    private void OnMovementCancelled()
    {
        anim.SetFloat("MovementBlend", 0f);

        moveVector = Vector2.zero;
        //Debug.Log("STOP MOVEMENT");
    }
}
