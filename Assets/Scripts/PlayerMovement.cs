using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input Checks")]
    private Vector2 moveVector;
    //private Vector2 lookVector;

    [Header("jump")]
    private float jumpForce = 12f;
    private float airMultiPlier = 0.4f;

    [Header("Sprint")]
    private float walkSpeed = 5f;
    private float sprintSpeed = 25f;
    //private float wallRunSpeed = 12f;
    private bool isSprinting;

    [Header("Restrictions")]
    private float drag = 3;

    [Header("Detections and checks")]
    private float playerHeigt = 1.6f;
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Components")]
    private Rigidbody rb;

    [Header("rotation")]
    private float rotationSpeed = 4f;
    private Transform cameraHolder;
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
        Debug.Log(isGrounded);
        if (isGrounded)
        {
            rb.linearDamping = drag;
        }
        else
        {
            rb.angularDamping = 3;
        }

    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeigt * 0.5f + 0.2f, whatIsGround);
        Vector3 move = new Vector3(moveVector.x,0,moveVector.y);
        move = cameraHolder.forward * move.z + cameraHolder.right * move.x;
        move.y = 0;
        if (isGrounded)
        {
            if (!isSprinting)
            {
                rb.AddForce(move * walkSpeed * 10, ForceMode.Force);
            }
            else
            {
                rb.AddForce(move * sprintSpeed * 10, ForceMode.Force);
            }
        }
        else
        {
            if (!isSprinting)
            {
                rb.AddForce(move * walkSpeed * airMultiPlier * 10, ForceMode.Force);
            }
            else
            {
                rb.AddForce(move * sprintSpeed * airMultiPlier * 10, ForceMode.Force);
            }
        }

        if(moveVector != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg + cameraHolder.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation, rotationSpeed * Time.deltaTime);
        }
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
                //rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
    }
    #endregion

}
