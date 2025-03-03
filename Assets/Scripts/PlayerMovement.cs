using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input Checks")]
    private Vector2 moveVector;

    [Header("jump")]
    private float jumpSpeed = 3f;
    private float jumpForce = 20f;
    private float airMultiPlier = 0.4f;

    [Header("Sprint")]
    private float walkSpeed = 5f;
    private float sprintSpeed = 10f;
    private float wallRunSpeed;
    private bool isSprinting;

    [Header("Restrictions")]
    private float drag = 5;

    [Header("Detections and checks")]
    private float playerHeigt = 2f;
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround;
    private float wallDist;
    private float minJumpHeigt;
    private RaycastHit wallCheckLeft;
    private RaycastHit wallCheckRight;
    private bool wallLeft;
    private bool wallRight;

    [Header("Components")]
    private Rigidbody rb;

    [Header("rotation")]
    private float rotationSpeed = 2f;
    private Transform cameraHolder;


    [Header("physics adjustments")]
    float gravityScale = 5f;

    [Header("SLope")]
    private float slopeAngle = 40f;
    private RaycastHit slopeHit;
    private float slide = 8f;
    private bool exitSlope;

    [Header("wallRunning")]
    public LayerMask whatIsWall;
    private float wallrunForce;
    private float maxWallRunTime;
    private float wallRunTime;
    private bool isWallRunning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraHolder = Camera.main.transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeigt * 0.5f + 0.2f, whatIsGround);
        if (isGrounded)
        {
            rb.linearDamping = drag;
        }
        else
        {
            rb.linearDamping = 0;
        }
        WallChecks();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);

        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);
        move = cameraHolder.forward * move.z + cameraHolder.right * move.x;
        move.y = 0;

        if (OnSlope())
        {
            if (!isSprinting)
            {
                rb.AddForce(GetSlopeDir() * walkSpeed * 10, ForceMode.Force);
                if(rb.linearVelocity.y > 0)
                {
                    rb.AddForce(Vector3.down * slide, ForceMode.Force);
                }
            }
            else
            {
                rb.AddForce(GetSlopeDir() * sprintSpeed * 10, ForceMode.Force);
            }
        }

        //on Ground
        if (isGrounded)
        {
            if (!isSprinting)
            {
                rb.AddForce(move.normalized * walkSpeed * 10, ForceMode.Force);
            }
            else
            {
                rb.AddForce(move.normalized * sprintSpeed * 10, ForceMode.Force);
            }
        }
        //in Air
        else
        {
            if (!isSprinting)
            {
                rb.AddForce(move.normalized * walkSpeed * airMultiPlier * 10, ForceMode.Force);
            }
            else
            {
                rb.AddForce(move.normalized * sprintSpeed * airMultiPlier * 10, ForceMode.Force);
            }
        }

        if (isWallRunning)
        {
            WallRunning();
        }

        if (moveVector != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg + cameraHolder.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
            rb.useGravity = !OnSlope();
    }

    #region Slope
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeigt * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < slopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeDir()
    {
        return Vector3.ProjectOnPlane(moveVector, slopeHit.normal).normalized;
    }
    #endregion

    #region WallRun
    private void CheckWall()
    {
        wallRight = Physics.Raycast(transform.position, Vector3.right, out wallCheckRight, wallDist, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, Vector3.left, out wallCheckLeft, wallDist, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position,Vector3.down,minJumpHeigt,whatIsGround);
    }

    private void StartWallRunning()
    {
        isWallRunning = true;
    }
    private void WallRunning()
    {
        rb.useGravity = false;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        
        Vector3 wallNormal = wallRight ? wallCheckRight.normal : wallCheckLeft.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        rb.AddForce(wallForward * wallrunForce, ForceMode.Force);
    }
    private void StopWallRunning()
    {
        isWallRunning = false;
    }

    private void WallChecks()
    {
        if((wallLeft || wallRight) && moveVector.y > 0 && AboveGround())
        {
            if (!isWallRunning)
            {
                StartWallRunning();
            }
            else
            {
                if (isWallRunning)
                {
                    StopWallRunning();
                }
            }
        }
    }
    #endregion

    #region Inputs
    public void OnWalk(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    public void Onjump(InputAction.CallbackContext context)
    {
        if (isGrounded)
            if (context.started)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
    }
    #endregion

}
