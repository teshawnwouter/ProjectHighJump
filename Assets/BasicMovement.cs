using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class BasicMovement : MonoBehaviour
{
    private Rigidbody rb;

    private Vector2 moveVector;

    private float walkSpeed = 5f;

    private float sprintSpeed = 25f;
    private bool isSprinting;

    private bool isGrounded;
    [SerializeField] private float playerHeigt = 1.5f;
    [SerializeField] private LayerMask whatIsGround;

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

    public void OnWalk(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            moveVector = context.ReadValue<Vector2>();

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
}
