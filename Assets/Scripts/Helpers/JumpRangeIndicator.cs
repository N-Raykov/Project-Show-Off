using UnityEngine;

public class JumpRangeIndicator : MonoBehaviour
{
    [SerializeField] private float maxTimeToLive = 5f;
    [SerializeField] private bool alwaysRender = false;

    private LineRenderer lineRendererMax;
    private LineRenderer lineRendererMin;
    private PlayerMovement playerMovement;
    private float distToBottomOfSprite;
    private float terminalRunningSpeed;
    private float jumpForceContinous;
    private int linePoints;
    private float gravityFallModifier;
    private float jumpForceInitial; 
    private float gravity;
    private float maxJumpTime;
    private float currentTimeAlive = 0;

    public void Initialize(PlayerMovement pPlayerMovement, float pDistToBottomOfSprite, float pTerminalRunningSpeed, float pJumpForceContinous, int pLinePoints, float pGravityFallModifier, float pJumpForceInitial, float pGravity, float pMaxJumpTime)
    {
        playerMovement = pPlayerMovement;
        distToBottomOfSprite = pDistToBottomOfSprite;
        terminalRunningSpeed = pTerminalRunningSpeed;
        jumpForceContinous = pJumpForceContinous;
        linePoints = pLinePoints;
        gravityFallModifier = pGravityFallModifier;
        jumpForceInitial = pJumpForceInitial;
        gravity = pGravity;
        maxJumpTime = pMaxJumpTime;

        Draw();
    }

    private void Awake()
    {
        lineRendererMax = GetComponentsInChildren<LineRenderer>()[0];
        lineRendererMin = GetComponentsInChildren<LineRenderer>()[1];
        if (lineRendererMax == null || lineRendererMin == null)
        {
            throw new System.Exception("There is a missing LineRenderer component.");
        }
    }

    private void FixedUpdate()
    {
        if (!alwaysRender)
        {
            DestroyAfterTime();
        }
        else
        {
            Draw();
        }
    }

    private void Draw()
    {
        DrawTrajectory(lineRendererMax, true, true, true, true);
        DrawTrajectory(lineRendererMin, false, false, true, true);
    }

    private void DrawTrajectory(LineRenderer pLineRenderer, bool pMaxRunningSpeed, bool pMaxJump, bool pMaxForwardInAir, bool pUseActualDirection)
    {
        Vector2 moveDirection = pUseActualDirection && playerMovement.moveVector != new Vector2(0, 0) ? playerMovement.moveVector : new Vector2(0, -1);

        pLineRenderer.enabled = true;
        pLineRenderer.positionCount = Mathf.CeilToInt(linePoints / Time.fixedDeltaTime) + 1;

        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y - (alwaysRender ? distToBottomOfSprite : 0), transform.position.z);

        Vector3 startJumpVelocity = new Vector3(0, jumpForceInitial, 0); //Debug.Log("startJumpVelocity: "+ startJumpVelocity);

        Vector2 startMovement = moveDirection * (pMaxRunningSpeed ? terminalRunningSpeed : 0);
        Vector3 terminalRunningVelocity = new Vector3(
            startMovement.x,
            0,
            startMovement.y); //Debug.Log("terminalRunningVelocity: "+ terminalRunningVelocity);

        Vector3 startVelocity = terminalRunningVelocity + startJumpVelocity; //Debug.Log("startVelocity: " + startVelocity);

        int i = 0;
        float lastYPoint = 0;
        pLineRenderer.SetPosition(i, startPosition);
        Vector3 simulatedForce = new Vector3();
        for (float time = 0; time < linePoints; time += Time.fixedDeltaTime)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;

            if (time < maxJumpTime && pMaxJump)
            {
                simulatedForce = new Vector3(
                    simulatedForce.x,
                    simulatedForce.y + jumpForceContinous * Time.fixedDeltaTime,
                    simulatedForce.z);
            }

            float currentGravity = gravity * (point.y < lastYPoint ? gravityFallModifier : 1);
            simulatedForce = new Vector3(
                simulatedForce.x,
                simulatedForce.y - currentGravity * Time.fixedDeltaTime,
                simulatedForce.z); //Debug.Log("2:" + (-currentGravity * Time.fixedDeltaTime));

            if (pMaxForwardInAir)
            {
                Vector3 currentMovement = moveDirection * playerMovement.movementSpeed;
                simulatedForce = new Vector3(
                    simulatedForce.x + currentMovement.x * Time.fixedDeltaTime,
                    simulatedForce.y,
                    simulatedForce.z + currentMovement.y * Time.fixedDeltaTime);
            }

            simulatedForce = new Vector3(
                simulatedForce.x * Mathf.Clamp01(1 - playerMovement.airDrag * Time.fixedDeltaTime),
                simulatedForce.y,
                simulatedForce.z * Mathf.Clamp01(1 - playerMovement.airDrag * Time.fixedDeltaTime));

            point = startPosition + startVelocity * time + (simulatedForce / 2f * time * time);

            lastYPoint = point.y;
            pLineRenderer.SetPosition(i, point);
        }       
    }

    private void DestroyAfterTime()
    {
        currentTimeAlive += Time.fixedDeltaTime;
        if (maxTimeToLive <= currentTimeAlive)
        {
            Destroy(gameObject);
        }
    }


    /*
Vector3[] trajectory = Plot(transform.position, 500, true, true, true, true);
lineRendererMax.positionCount = trajectory.Length;

Vector3[] positions = new Vector3[trajectory.Length];
for (int i = 0; i < trajectory.Length; i++)
{
    positions[i] = trajectory[i];
}

lineRendererMax.SetPositions(positions);
private Vector3[] Plot(Vector3 pPosition, int pSteps, bool pMaxRunningSpeed, bool pMaxJump, bool pMaxForwardInAir, bool pUseActualDirection)
{
Vector3[] results = new Vector3[pSteps];

Vector3 startJumpVelocity = new Vector3(0, jumpForceInitial, 0); //Debug.Log("startJumpVelocity: "+ startJumpVelocity);

Vector2 startMovement = (pUseActualDirection ? playerMovement.moveVector : new Vector2(0, -1)) * (pMaxRunningSpeed ? terminalRunningSpeed : 0);
Vector3 terminalRunningVelocity = new Vector3(
    startMovement.x,
    0,
    startMovement.y); //Debug.Log("terminalRunningVelocity: "+ terminalRunningVelocity);

Vector3 startVelocity = terminalRunningVelocity + startJumpVelocity; //Debug.Log("startVelocity: " + startVelocity);

float dragForce = Mathf.Clamp01(1 - playerMovement.airDrag * Time.fixedDeltaTime);

for (int i = 0; i < pSteps; i++)
{
    Vector3 continousJumpVelocity = new Vector3();
    if (pSteps < maxJumpTime && pMaxJump)
    {
        continousJumpVelocity = new Vector3(
            0,
            jumpForceContinous * Time.fixedDeltaTime,
            0);
    }

    float currentGravity = gravity;
    Vector3 gravityVelocity = new Vector3(
        0,
        -currentGravity * Time.fixedDeltaTime,
        0); //Debug.Log("2:" + (-currentGravity * Time.fixedDeltaTime));

    Vector3 moveInAirVelocity = new Vector3();
    if (pMaxForwardInAir)
    {
        Vector3 currentMovement = (pUseActualDirection ? playerMovement.moveVector : new Vector2(0, -1)) * playerMovement.movementSpeed;
        moveInAirVelocity = new Vector3(
            currentMovement.x * Time.fixedDeltaTime,
            0,
            currentMovement.y * Time.fixedDeltaTime);
    }

    Vector3 moveStep = (continousJumpVelocity + gravityVelocity + moveInAirVelocity) * dragForce;
    pPosition += startVelocity;
    pPosition += moveStep;
    results[i] = pPosition;
}

return results;
}
    */
}
