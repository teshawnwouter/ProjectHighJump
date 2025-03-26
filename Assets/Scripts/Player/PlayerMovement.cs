using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    #region variables
    [Header("Input Checks")]
    public Vector2 moveVector;

    [Header("jump")]
    private float jumpForce = 18f;
    private float airMultiPlier = 0.4f;

    [Header("Sprint")]
    private float walkSpeed = 7f;
    private float sprintSpeed = 14f;
    public float wallRunSpeed = 12f;
    private bool isSprinting;

    [Header("Restrictions")]
    private float drag = 5;

    [Header("Detections and checks")]
    private float playerHeigt = 2f;
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround;
    public bool wallRunning;

    [Header("Components")]
    private Rigidbody rb;

    [Header("rotation")]
    private float rotationSpeed = 4f;

    [Header("physics adjustments")]
    float gravityScale = 5f;

    [Header("SLope")]
    private float slopeAngle = 40f;
    private RaycastHit slopeHit;
    private float slide = 8f;
    private bool exitSlope;

    [Header("reffrences")]
    private Transform cameraHolder;
    private WallRun wallRun;
    private Swinging swing;

    [Header("grapple")]
    public bool isGrappling;
    public bool freeze;
    private Vector3 velocityToSet;

    [Header("Swing")]
    public bool isSwining;
    #endregion

    #region Runtime
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        swing = GetComponent<Swinging>();
        wallRun = GetComponent<WallRun>();
        cameraHolder = Camera.main.transform;
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeigt * 0.5f + 0.2f, whatIsGround);
        if (isGrounded && !isGrappling)
        {
            rb.linearDamping = drag;
        }
        else
        {
            rb.linearDamping = 0;
        }
        if (freeze)
        {
            rb.linearVelocity = Vector3.zero;

        }
    }

    private void FixedUpdate()
    {
        if (!wallRunning || !isGrappling)
        {
            rb.AddForce(Physics.gravity * gravityScale * rb.mass);

        }

        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);
        move = cameraHolder.forward * move.z + cameraHolder.right * move.x;
        move.y = 0;

        if (OnSlope())
        {
            if (!isSprinting)
            {
                rb.AddForce(GetSlopeDir() * walkSpeed * 10, ForceMode.Force);
                if (rb.linearVelocity.y > 0)
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
            if (!wallRunning)
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
        }

        if (!wallRunning || !isGrappling)
        {
            if (moveVector != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg + cameraHolder.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                transform.localRotation = Quaternion.Slerp(rb.rotation, rotation, rotationSpeed * Time.deltaTime);
            }

        }

        rb.useGravity = !OnSlope();
    }
    #endregion

    #region grapple

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        //made with https://www.youtube.com/watch?v=IvT8hjy6q4o for equations and guided with https://www.youtube.com/watch?v=TYzZsBl3OI0
        float gravity = Physics.gravity.y * gravityScale;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
    private void SetVelocity()
    {   
        rb.linearVelocity = velocityToSet;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        isGrappling = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

    }
    #endregion

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

    #region Inputs
    public void OnWalk(InputAction.CallbackContext context)
    {
        if (!isGrappling)
        {
            if (context.performed || context.canceled)
            {
                moveVector = context.ReadValue<Vector2>();
            }
        }
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

        if (wallRunning)
        {
            if (context.performed)
            {
                wallRun.WallJump();
            }
        }
        if (isSwining)
        {
            swing.StopSwing();
            rb.linearVelocity = new Vector3(rb.linearVelocity.x,rb.linearVelocity.y + jumpForce,rb.linearVelocity.z);
        }

    }
    #endregion

}
