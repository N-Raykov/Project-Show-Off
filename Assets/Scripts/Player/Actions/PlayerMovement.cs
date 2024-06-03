using UnityEngine;

public class PlayerMovement : AbstractPlayerAction
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] public float movementSpeed = 135f;
    [SerializeField] public float groundDrag = 8f;
    [SerializeField] public float airDrag = 6f;

    private Vector3 currentMovement;

    private Camera mainCamera;
    public Vector2 moveVector;

    private float t;


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
        if(anim != null)
        {
            anim.SetBool("isGrounded", IsGrounded());
        }

        //Debug.Log("isGrounded: "+ isGrounded);
        //if(isGrounded)
        HandleMovement();


        HandleDrag();

        //Debug.Log("Player Velocity: " + rb.velocity);
    }

    private void HandleMovement()
    {
        currentMovement = moveVector * movementSpeed;

        rb.velocity = new Vector3(
            rb.velocity.x + currentMovement.x * Time.fixedDeltaTime,
            rb.velocity.y,
            rb.velocity.z + currentMovement.y * Time.fixedDeltaTime);
    }
    private void HandleDrag()
    {
        rb.velocity = new Vector3(
            rb.velocity.x * Mathf.Clamp01(1 - (isGrounded ? groundDrag : airDrag) * Time.fixedDeltaTime),
            rb.velocity.y,
            rb.velocity.z * Mathf.Clamp01(1 - (isGrounded ? groundDrag : airDrag) * Time.fixedDeltaTime));
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
        
        //Debug.Log("START MOVEMENT: " + moveVector);
    }

    private void OnMovementCancelled()
    {
        anim.SetFloat("MovementBlend", 0f);

        moveVector = Vector2.zero;
        //Debug.Log("STOP MOVEMENT");
    }
}
