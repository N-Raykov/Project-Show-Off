using UnityEngine;

public class JumpRangeIndicator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 startPosition;
    private float terminalSpeed;
    private float addedJumpForce;
    private int linePoints;
    private float timeBetweenPoints;
    private float gravityFallModifier;
    private float jumpForceInitial;
    private float gravity;
    private float airDrag;

    public void Initialize(Vector3 pStartPosition, float pTerminalSpeed, float pAddedJumpForce, int pLinePoints, float pTimeBetweenPoints, float pGravityFallModifier, float pJumpForceInitial, float pGravity, float pAirDrag)
    {
        startPosition = pStartPosition;
        terminalSpeed = pTerminalSpeed;
        addedJumpForce = pAddedJumpForce;
        linePoints = pLinePoints;
        timeBetweenPoints = pTimeBetweenPoints;
        gravityFallModifier = pGravityFallModifier;
        jumpForceInitial = pJumpForceInitial;
        gravity = pGravity;
        airDrag = pAirDrag;
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            throw new System.Exception("There is no LineRenderer component.");
        }
    }

    private void FixedUpdate()
    {
        DrawTrajectory();
    }

    private void DrawTrajectory()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;

        Vector3 maxRunningVelocity = new Vector3(0, 0, -terminalSpeed);
        Vector3 jumpStartVelocity = new Vector3(0, jumpForceInitial, 0);
        Vector3 addedJumpVelocity = new Vector3(0, addedJumpForce, 0);
        //addedJumpVelocity = new Vector3();
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
}
