using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleHook : MonoBehaviour
{
    [Header("grappel")]
    private float maxGrappleDist = 25f;
    private float grappleDelay = 0.01f;
    private Vector3 grappleHitPoint;
    private float overshootYAxis = 2f;

    [Header("Restrictions")]
    private float grappleCD = 1f;
    private float grappleCDTimer;

    [Header("referances")]
    public LayerMask whatIsGrapple;
    public LineRenderer lr;
    public Transform grapplePoint;
    private Transform camForward;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        camForward = Camera.main.transform;
    }

    private void Update()
    {
        if (grappleCDTimer > 0)
        {
            grappleCDTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (playerMovement.isGrappling)
        {
            lr.SetPosition(0, grapplePoint.position);
        }
    }

    private void StartGrapple()
    {

        if (grappleCDTimer > 0) return;

        playerMovement.isGrappling = true;

        RaycastHit hit;
        if (Physics.Raycast(grapplePoint.position, camForward.forward, out hit, maxGrappleDist, whatIsGrapple))
        {
            grappleHitPoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelay);
        }
        else
        {
            grappleHitPoint = grapplePoint.position + camForward.forward * maxGrappleDist;

            Invoke(nameof(StopGrapple), 3f);
        }
        transform.forward = camForward.forward;
        lr.enabled = true;
        lr.SetPosition(1, grappleHitPoint);
    }

    private void ExecuteGrapple()
    {
        playerMovement.freeze = false;
        
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grappleHitPoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        playerMovement.JumpToPosition(grappleHitPoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);

    }

    private void StopGrapple()
    {
        playerMovement.freeze = false;
        playerMovement.isGrappling = false;
        grappleCDTimer = grappleCD;
        lr.enabled = false;
    }

    public void OnGrappleShot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartGrapple();
        }
    }
}
