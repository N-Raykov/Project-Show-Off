using UnityEngine;

public class PlayerJump : AbstractPlayerAction
{
    [Header("Jumping")]
    [SerializeField] private PlayerInputReader reader;
    [SerializeField] private float jumpForceInitial = 10f;
    [SerializeField] private float jumpForceContinous = 1f;
    [SerializeField] private float maxJumpTime = 0.3f;
    [SerializeField] private float gravity = 40f;
    [SerializeField] private float gravityFallModifier = 4f;
    [SerializeField] private float airDrag = 0.03f;

    [Header("Draw Trajectory")]
    [SerializeField] [Range(10, 100)] private int linePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;
    [SerializeField] GameObject rangeIndicatorPrefab;

    private LineRenderer lineRenderer;
    private float terminalSpeed;
    private float addedJumpForce;
    private bool isJumping;
    private float jumpTime = 0;
    private float currentGravity;

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

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            throw new System.Exception("There is no LineRenderer component.");
        }

        terminalSpeed = 0;
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            for (int i = 0; i <= 5000; i++)
            {
                terminalSpeed += playerMovement.movementSpeed * Time.fixedDeltaTime;
                terminalSpeed *= (1 - playerMovement.groundDrag * Time.fixedDeltaTime);
            }
        }
        //Debug.Log("terminalSpeed: "+ terminalSpeed);
        //14.75 max velocity

        addedJumpForce = 0;
        for (float i = 0; i <= maxJumpTime; i += Time.fixedDeltaTime)
        {
            addedJumpForce += jumpForceContinous * Time.fixedDeltaTime;
        }
        //Debug.Log("addedJumpForce: "+ addedJumpForce);
        //addedJumpForce = 0;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        HandleJumping();

        if (!isGrounded)
        {
            HandleAirDrag();
            HandleGravity();
        }

        DrawTrajectory();
    }

    private void DrawTrajectory()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;

        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y - distToBottomOfSprite, transform.position.z);

        Vector3 maxRunningVelocity = new Vector3(0, 0, -terminalSpeed);
        Vector3 jumpStartVelocity = new Vector3(0, jumpForceInitial, 0);
        Vector3 addedJumpVelocity = new Vector3(0, addedJumpForce, 0);
        Vector3 startVelocity = maxRunningVelocity + jumpStartVelocity + addedJumpVelocity;

        int i = 0;
        float lastYPoint = 0;
        lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;

            float simulatedForce = -gravity * (point.y <= lastYPoint ? gravityFallModifier : 1);
            point.y = startPosition.y + startVelocity.y * time + (simulatedForce / 2f * time * time);

            lastYPoint = point.y;
            lineRenderer.SetPosition(i, point);
        }
    }

    private void OnJumpPerformed()
    {
        if (!isGrounded)
            return;

        isJumping = true;

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForceInitial, rb.velocity.z);

        EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.PlayerJump));

        InitializeRangeIndicatorPrefab();

        //Debug.Log("START JUMP: " + jumpForce);
        //Debug.Log("Velocity at jump start: " + rb.velocity);
    }

    private void InitializeRangeIndicatorPrefab()
    {
        GameObject prefab = Instantiate(rangeIndicatorPrefab);
        prefab.transform.position = new Vector3(transform.position.x, transform.position.y - distToBottomOfSprite, transform.position.z);
        JumpRangeIndicator jumpRangeIndicator = prefab.GetComponent<JumpRangeIndicator>();
        if (jumpRangeIndicator != null)
        {
            jumpRangeIndicator.Initialize(prefab.transform.position, terminalSpeed, addedJumpForce, linePoints, timeBetweenPoints, gravityFallModifier, jumpForceInitial, gravity, airDrag);
        }
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

        //addedJumpForce += jumpForceContinous * Time.fixedDeltaTime;
        //Debug.Log("addedJumpForce: "+ addedJumpForce);

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
            anim.SetFloat("JumpingBlend", (rb.velocity.y <= 0 ? 1 : 0));
        }

        currentGravity = gravity * (rb.velocity.y <= 0 ? gravityFallModifier : 1);
        rb.velocity = new Vector3(
            rb.velocity.x,
            rb.velocity.y - currentGravity * Time.fixedDeltaTime,
            rb.velocity.z);
    }

    private void HandleAirDrag()
    {
        rb.velocity = new Vector3(
            rb.velocity.x * (1 - airDrag * Time.fixedDeltaTime),
            rb.velocity.y,
            rb.velocity.z * (1 - airDrag * Time.fixedDeltaTime));
    }
}
