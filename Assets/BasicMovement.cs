using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody),typeof(PlayerInput))]
public class BasicMovement : MonoBehaviour
{
    private Rigidbody rb;

    private Vector2 moveVector;
    private float walkSpeed;
    private float sprintSpeed;
    private bool isSprinting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
       
    }

    private void FixedUpdate()
    {
        Debug.Log(moveVector.x);
        rb.linearVelocity = new Vector3(moveVector.x * walkSpeed, 0 , moveVector.y * walkSpeed);
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveVector = context.ReadValue<Vector2>();
            
        }
    }
}
