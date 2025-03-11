using UnityEngine;
using UnityEngine.InputSystem;

public class Swinging : MonoBehaviour
{
    [Header("Swing")]
    private float maxSwingDist = 25f;
    private Vector3 swingPoint;
    public SpringJoint joint;
    private Vector3 currentGrapplePos;
    private float horizontalThrustForce;
    private float forwardThrustForce;
    private float extendedCableSpeed;


    [Header("Refrerances")]
    public LineRenderer lineRenderer;
    public Transform SwingHookPoint;
    private Transform camForward;
    public LayerMask SwingAble;
    private PlayerMovement pm;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        camForward = Camera.main.transform;
    }
    private void StartSWing()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, camForward.forward, out hit, maxSwingDist, SwingAble))
        {
            swingPoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distFromPoint = Vector3.Distance(transform.position,swingPoint);

            joint.maxDistance = distFromPoint * 0.8f;
            joint.minDistance = distFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

        }
    }
    
    private void StopSwing()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    private void DrawRope()
    {
        if (!joint) return;

        currentGrapplePos = Vector3.Lerp(currentGrapplePos, swingPoint, Time.deltaTime * 8f);

        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,swingPoint);
    }


    private void SwingMovement()
    {
        if (pm.moveVector.x < 0)
        {
            rb.AddForce(-transform.right * horizontalThrustForce * Time.deltaTime);
        }
        if(pm.moveVector.x > 0)
        {
            rb.AddForce(transform.right * horizontalThrustForce * Time.deltaTime);
        }
        if(pm.moveVector.y < 0)
        {
            rb.AddForce(transform.forward * forwardThrustForce * Time.deltaTime);
        }
    }

    public void SwingJump()
    {
        Vector3 distanceToPoint = swingPoint - transform.position;
        rb.AddForce(distanceToPoint.normalized * forwardThrustForce * Time.deltaTime);

        float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;
    }

    public void OnSwingHook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartSWing();
        }
        if (context.canceled)
        {
            StopSwing();
        }
    }
}
