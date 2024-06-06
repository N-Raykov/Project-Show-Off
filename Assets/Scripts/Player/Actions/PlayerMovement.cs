using UnityEngine;

public struct JumpIndicatorDataPlayerMovement
{
    public float movementSpeed;
    public float airDrag;
    public float groundDrag;
    public Vector2 moveVector;

    public JumpIndicatorDataPlayerMovement(float pMovementSpeed, float pAirDrag, float pGroundDrag, Vector2 pMoveVector)
    {
        movementSpeed = pMovementSpeed;
        airDrag = pAirDrag;
        groundDrag = pGroundDrag;
        moveVector = pMoveVector;
    }
}

public class PlayerMovement : AbstractPlayerAction
{
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private ParticleSystem particleEffect;
    [SerializeField] private float movementSpeed = 100f;
    [SerializeField] private float groundDrag = 0.1f;
    [SerializeField] private float airDrag = 6f;

    private Camera mainCamera;
    private Vector3 currentMovement;
    private Vector2 moveVector;

    private bool _isEmittingParticles;

    public JumpIndicatorDataPlayerMovement GetJumpIndicatorData()
    {
        return new JumpIndicatorDataPlayerMovement(movementSpeed, airDrag, groundDrag, moveVector);
    }

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

        HandleMovement();
        HandleDrag();
    }

    private void HandleMovement()
    {
        currentMovement = moveVector * movementSpeed;

        rb.velocity = new Vector3(
            rb.velocity.x + currentMovement.x * Time.fixedDeltaTime,
            rb.velocity.y,
            rb.velocity.z + currentMovement.y * Time.fixedDeltaTime);

        if (isGrounded && currentMovement.magnitude > 0 && _isEmittingParticles == false)
        {
            particleEffect.Play();
            _isEmittingParticles = true;
        }
        else if (isGrounded == false && _isEmittingParticles == true || currentMovement.magnitude == 0 && _isEmittingParticles == true)
        {
            particleEffect.Stop();
            _isEmittingParticles = false;
        }
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
        moveVector = new Vector2(desiredMoveDirection.x, desiredMoveDirection.z); //Debug.Log("START MOVEMENT: " + moveVector);
    }

    private void OnMovementCancelled()
    {
        anim.SetFloat("MovementBlend", 0f);

        moveVector = Vector2.zero; //Debug.Log("STOP MOVEMENT");
    }
}
