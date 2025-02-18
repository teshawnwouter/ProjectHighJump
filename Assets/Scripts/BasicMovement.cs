using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class BasicMovement : MonoBehaviour
{
    private Rigidbody rb;

    private Vector2 moveVector;
    private Vector2 lookVector;

    private float walkSpeed = 5f;
    private float sprintSpeed = 25f;
    private float wallRunSpeed = 12f;
    private bool isSprinting;

    private float jumpForce = 5f;

    private bool isGrounded;
    private bool isOnWall;
    private float playerHeigt = 1.6f;
    [SerializeField] private LayerMask whatIsGround;

    float rotationspeed = 4f;

    [Header("Wallrun")]
    public LayerMask whatIsWall;
    public float wallForce;
    public float maxWallRunTime;
    private float wallRunTime;

    [Header("detections")]
    public float wallCheckDetection;
    public float minwallJump;
    public RaycastHit wallLeftHit;
    public RaycastHit wallRightHit;
    public bool onWallRight;
    public bool onWallLeft;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeigt * 0.5f + 0.2f, whatIsGround);
        if (isGrounded)
        {
            if (!isSprinting)
            {
                rb.linearVelocity = new Vector3(moveVector.x * walkSpeed, 0, moveVector.y * walkSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector3(moveVector.x * sprintSpeed, 0, moveVector.y * sprintSpeed);
            }
        }

        if ((onWallLeft || onWallRight) && AboveGround())
        {

        }

        if (rb.linearVelocity != Vector3.zero)
        {
            float targetRotation = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Quaternion turning = Quaternion.Euler(0f, targetRotation, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, turning, Time.deltaTime * rotationspeed);
        }

        if (isOnWall)
            WallRunMovement();
    }
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
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }
    #endregion

    private void CheckForWall()
    {
        onWallRight = Physics.Raycast(transform.position, transform.right, out wallRightHit, wallCheckDetection, whatIsWall);
        onWallLeft = Physics.Raycast(transform.position, transform.right, out wallLeftHit, wallCheckDetection, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minwallJump, whatIsGround);
    }

    private void StartWallRun()
    {
        isOnWall = true;
    }

    private void WallRunMovement()
    {
        rb.useGravity = false;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        Vector3 wallNormal = onWallRight ? wallRightHit.normal : wallLeftHit.normal;

        Vector3 forwardMovementOnWal = Vector3.Cross(wallNormal, transform.up);

        if ((transform.forward - forwardMovementOnWal).magnitude > (transform.forward - -forwardMovementOnWal).magnitude)
            forwardMovementOnWal = -forwardMovementOnWal;

            rb.AddForce(forwardMovementOnWal * wallForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        isOnWall = false;
    }

}
