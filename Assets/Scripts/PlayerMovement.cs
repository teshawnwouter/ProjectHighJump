using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input Checks")]
    public Vector2 moveVector;

    [Header("jump")]
    private float jumpForce = 18f;
    private float airMultiPlier = 0.4f;

    [Header("Sprint")]
    private float walkSpeed = 5f;
    private float sprintSpeed = 10f;
    public float wallRunSpeed = 8f;
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
    private float rotationSpeed = 2f;
    private Transform cameraHolder;


    [Header("physics adjustments")]
    float gravityScale = 5f;

    [Header("SLope")]
    private float slopeAngle = 40f;
    private RaycastHit slopeHit;
    private float slide = 8f;
    private bool exitSlope;


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
