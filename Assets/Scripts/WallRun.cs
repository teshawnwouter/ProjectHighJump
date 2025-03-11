using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("wallRunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    private float wallRunForce = 18f;
    private float wallRunTimer;
    private float wallRunMaxTime = 1.5f;

    [Header("detections")]
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private float wallCheckDist = 1f;
    private float minJumpHeigt = 1.5f;
    private bool wallLeft;
    private bool wallRight;

    [Header("Refereces")]
    private Rigidbody rb;
    private PlayerMovement playerMovement;

    [Header("WallJump")]
    private float wallJumpForce = 7f;
    private float wallSlideForce = 12f;

    [Header("exiting")]
     private bool isExitingWall;
     private float exitWallTime = 0.2f;
     private float exitWallTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        WallChecker();
        StateMachine();

        if (isExitingWall)
        {
            if (playerMovement.wallRunning)
            {
                StopWallRun();
            }

            if (exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            if(exitWallTimer <= 0)
            {
                isExitingWall = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerMovement.wallRunning)
        {
            WallRunning();
        }
    }

    private void WallChecker()
    {
        wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallCheckDist, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallCheckDist, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeigt, whatIsGround);
    }

    private void StateMachine()
    {
        if ((wallLeft || wallRight) && playerMovement.moveVector.y > 0 && AboveGround() && !isExitingWall)
        {
            if (!playerMovement.wallRunning)
                StartWallRun();
        }
        else
        {
            if (playerMovement.wallRunning)
            {
                StopWallRun();
            }
        }
    }


    private void StartWallRun()
    {
        playerMovement.wallRunning = true;
        rb.useGravity = false;
    }
    private void WallRunning()
    {
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForwardDirection = Vector3.Cross(wallNormal, transform.up);


        if ((transform.forward - wallForwardDirection).magnitude > (transform.forward - -wallForwardDirection).magnitude)
        {
            wallForwardDirection = -wallForwardDirection;
        }

        if ((wallLeft && playerMovement.moveVector.x > 0) && (wallRight && playerMovement.moveVector.x < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        rb.linearVelocity = wallForwardDirection * wallRunForce;

        rb.AddForce(wallForwardDirection * wallRunForce, ForceMode.Force);

        transform.rotation = Quaternion.LookRotation(wallForwardDirection, transform.up);
    }

    private void StopWallRun()
    {
        playerMovement.wallRunning = false;
        rb.useGravity = true;
    }

    public void WallJump()
    {
        isExitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpForce + wallNormal * wallSlideForce;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
