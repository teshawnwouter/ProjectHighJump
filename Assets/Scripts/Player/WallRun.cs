using Unity.Cinemachine;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    #region Variables
    [Header("wallRunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    private float wallRunForce = 18f;

    [Header("detections")]
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private float wallCheckDist = 1f;
    private float minJumpHeigt = 0.5f;
    private bool wallLeft;
    private bool wallRight;

    [Header("Refereces")]
    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private CinemachineCamera cam;

    [Header("WallJump")]
    private float wallJumpForce = 9f;
    private float wallSlideForce = 15f;

    [Header("exiting")]
     private bool isExitingWall;
     private float exitWallTime = 0.2f;
     private float exitWallTimer;

    [Header("camPositions")]
    public Transform camLeft;
    public Transform camRight;

    #endregion

    #region runtime
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        cam = FindFirstObjectByType<CinemachineCamera>();
    }
    private void Update()
    {
        WallChecker();
        StateMachine();
        CamAdjustment();

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
    #endregion

    #region Checkkers
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
    #endregion

    #region Physiscs and wallrun mechanic
    private void StartWallRun()
    {
        playerMovement.wallRunning = true;
        rb.useGravity = false;
    }

    /// <summary>
    /// Sets up the wall running but pushing me towards the wall and giving me acces to move to the walls normal
    /// </summary>
    private void WallRunning()
    {
        //checks if the wall is on the left or the right
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        //crosses my up possition with the wall that is being detected
        Vector3 wallForwardDirection = Vector3.Cross(wallNormal, transform.up);

        //if i am facing forward and the normal/ z pos of the wall is behind me turn the wall forward  backwards
        if ((transform.forward - wallForwardDirection).magnitude > (transform.forward - -wallForwardDirection).magnitude)
        {
            wallForwardDirection = -wallForwardDirection;
        }

        //takes my input as long as its towards the wall it keeps me on the wall
        if ((wallLeft && playerMovement.moveVector.x > 0) && (wallRight && playerMovement.moveVector.x < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        //puts my velocity towards the normal of the wall with the run force giving me a wall run on curves in both directions
        rb.linearVelocity = wallForwardDirection * wallRunForce;

        //gives me force towards the wall normal
        rb.AddForce(wallForwardDirection * wallRunForce, ForceMode.Force);

        //turns me towards the direction of the wall its normal so i look where i am going
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

        //takes the wall its normal direction
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        //take my up and adds the wall force  towards the wall normal so i kick off the wall
        Vector3 forceToApply = transform.up * wallJumpForce + wallNormal * wallSlideForce;

        //keeps my velocity in all but the z acces so i dont go up
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        // aplys the force to the normal i calculated
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    /// <summary>
    /// Adjusts the priority
    /// </summary>
    private void CamAdjustment()
    {
        if (wallRight) 
        { 

        }else
        {

        }

    }
    #endregion
}
