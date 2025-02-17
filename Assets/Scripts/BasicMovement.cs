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
    private bool isSprinting;

    private float jumpForce = 5f;

    private bool isGrounded;
    private float playerHeigt = 1.6f;
    [SerializeField] private LayerMask whatIsGround;

    private float rotationSpeed = 2.2f;

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
    }

    private void Update()
    {
        transform.Rotate(0, lookVector.x * rotationSpeed, 0f);
    }

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

}
