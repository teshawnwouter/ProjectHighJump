using Unity.VisualScripting;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("wallRunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    private float wallRunForce = 8;
    private float wallRunTimer;
    private float wallRunMaxTime = 1.5f;

    [Header("detections")]
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private float wallCheckDist = 0.7f;
    private float minJumpHeigt;
    private bool wallLeft;
    private bool wallRight;

    [Header("Refereces")]
    public Transform orientation;
    private Rigidbody rb;
    private PlayerMovement playerMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        WallChecker();
        StateMachine();
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
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDist, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDist, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeigt, whatIsGround);
    }

    private void StateMachine()
    {
        if((wallLeft || wallRight) && playerMovement.moveVector.y > 0 && AboveGround())
        {
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
    }
    private void WallRunning()
    {
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x,0,rb.linearVelocity.z);

        Vector3 wallNormal = wallRight ? leftWallHit.normal : leftWallHit.normal;

        Vector3 wallForwardDirection = Vector3.Cross(wallNormal,transform.up);

        if((orientation.forward - wallForwardDirection).magnitude >(orientation.forward - -wallForwardDirection).magnitude)
        {
            wallForwardDirection = -wallForwardDirection;
        }

        if (!(wallLeft && playerMovement.moveVector.y > 0) && !(wallRight && playerMovement.moveVector.y < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        rb.AddForce(wallForwardDirection * wallRunForce,ForceMode.Force);
    }

    private void StopWallRun()
    {
        playerMovement.wallRunning = false;
    }

}
