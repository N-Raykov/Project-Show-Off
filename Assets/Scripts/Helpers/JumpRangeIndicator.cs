using UnityEngine;

public class JumpRangeIndicator : MonoBehaviour
{
    [SerializeField] private float timeToLive = 5f;
    [SerializeField] private bool alwaysRender = false;
    [SerializeField] private bool maxRunningSpeedGreenCurve = true;
    [SerializeField] private bool maxJumpDistanceGreenCurve = true;
    [SerializeField] private bool maxForwardInAirGreenCurve = true;
    [SerializeField] private bool useActualDirectionGreenCurve = true;
    [SerializeField] private bool maxRunningSpeedRedCurve = true;
    [SerializeField] private bool maxJumpDistanceRedCurve = false;
    [SerializeField] private bool maxForwardInAirRedCurve = true;
    [SerializeField] private bool useActualDirectionRedCurve = true;

    private LineRenderer lineRendererGreen;
    private LineRenderer lineRendererRed;
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    private JumpIndicatorDataPlayerJump jumpIndicatorDataJump;
    private JumpIndicatorDataPlayerMovement jumpIndicatorDataMove;
    private float linePoints = 1f;
    private float currentTimeAlive;

    public void Initialize(PlayerMovement pPlayerMovement, PlayerJump pPlayerJump)
    {
        playerMovement = pPlayerMovement;
        playerJump = pPlayerJump;

        jumpIndicatorDataJump = playerJump.GetJumpIndicatorData();
        jumpIndicatorDataMove = playerMovement.GetJumpIndicatorData();

        Draw();
    }

    private void Awake()
    {
        lineRendererGreen = GetComponentsInChildren<LineRenderer>()[0];
        lineRendererGreen.startColor = Color.green;
        lineRendererGreen.endColor = Color.green;
        lineRendererRed = GetComponentsInChildren<LineRenderer>()[1];
        lineRendererRed.startColor = Color.red;
        lineRendererRed.endColor = Color.red;
        if (lineRendererGreen == null || lineRendererRed == null)
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
        else if(lineRendererGreen != null && lineRendererRed != null)
        {
            Draw();
        }
    }

    private void Draw()
    {
        DrawTrajectory(lineRendererGreen, maxRunningSpeedGreenCurve, maxJumpDistanceGreenCurve, maxForwardInAirGreenCurve, useActualDirectionGreenCurve);
        DrawTrajectory(lineRendererRed, maxRunningSpeedRedCurve, maxJumpDistanceRedCurve, maxForwardInAirRedCurve, useActualDirectionRedCurve);
    }

    private void DrawTrajectory(LineRenderer pLineRenderer, bool pMaxRunningSpeed, bool pMaxJump, bool pMaxForwardInAir, bool pUseActualDirection)
    {
        if (pLineRenderer == null || playerJump == null || playerMovement == null)
            return;

        jumpIndicatorDataMove = playerMovement.GetJumpIndicatorData();
        jumpIndicatorDataJump = playerJump.GetJumpIndicatorData();

        pLineRenderer.enabled = true;
        pLineRenderer.positionCount = Mathf.CeilToInt(linePoints / Time.fixedDeltaTime) + 2; //Debug.Log("pLineRenderer.positionCount: " + pLineRenderer.positionCount);

        Vector3 position = new Vector3(transform.position.x, transform.position.y - (alwaysRender ? jumpIndicatorDataJump.distToBottomOfSprite : 0), transform.position.z);

        Vector3 startJumpVelocity = new Vector3(0, jumpIndicatorDataJump.jumpForceInitial, 0); //Debug.Log("startJumpVelocity: " + startJumpVelocity);

        Vector2 moveDirection = pUseActualDirection && jumpIndicatorDataMove.moveVector != new Vector2(0, 0) ? jumpIndicatorDataMove.moveVector : new Vector2(0, -1);
        Vector2 startMovement = moveDirection * (pMaxRunningSpeed ? jumpIndicatorDataJump.terminalRunningSpeed : 0);
        Vector3 terminalRunningVelocity = new Vector3(
            startMovement.x,
            0,
            startMovement.y); //Debug.Log("terminalRunningVelocity: " + terminalRunningVelocity);

        Vector3 velocity = terminalRunningVelocity + startJumpVelocity; //Debug.Log("startVelocity: " + startVelocity);

        Vector3 gravityAcceleration = new Vector3(
            0,
            -jumpIndicatorDataJump.gravity,
            0); //Debug.Log("-gravity: " + (-gravity));

        Vector3 continousJumpAcceleration = new Vector3(
            0,
            jumpIndicatorDataJump.jumpForceContinous,
            0);

        Vector3 currentMovement = moveDirection * jumpIndicatorDataMove.movementSpeed;
        Vector3 moveInAirAcceleration = new Vector3(
            currentMovement.x,
            0,
            currentMovement.y); //Debug.Log("moveInAirVelocity: " + moveInAirVelocity);       

        int i = 0;
        float lastY = position.y;
        pLineRenderer.SetPosition(i, position); //Debug.Log("i: " + i + " startPosition: " + startPosition);
        for (float time = 0; time < linePoints; time += Time.fixedDeltaTime)
        {
            i++;
            
            if(i > 1)
            {
                velocity += gravityAcceleration * (lastY > position.y ? jumpIndicatorDataJump.gravityFallModifier : 1) * Time.fixedDeltaTime
                + (pMaxForwardInAir ? moveInAirAcceleration * Time.fixedDeltaTime : new Vector3())
                + (pMaxJump && time < jumpIndicatorDataJump.maxJumpTime ? continousJumpAcceleration * Time.fixedDeltaTime : new Vector3());

                float dragForce = Mathf.Clamp01(1 - jumpIndicatorDataMove.airDrag * Time.fixedDeltaTime);
                velocity.x *= dragForce;
                velocity.z *= dragForce;
            }

            lastY = position.y;
            position += velocity * Time.fixedDeltaTime;

            pLineRenderer.SetPosition(i, position); //Debug.Log("i: " + i + " point: " + point);
        }
    }
    private void DestroyAfterTime()
    {
        currentTimeAlive += Time.fixedDeltaTime;
        if (timeToLive <= currentTimeAlive)
        {
            Destroy(gameObject);
        }
    }
}
