using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleHook : MonoBehaviour
{
    #region Variables
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
    #endregion

    #region Runtime
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
    #endregion

    #region Grappel logic

    /// <summary>
    /// Starts the grapple itself via raycast
    /// </summary>
    private void StartGrapple()
    {
        //If the grapple timer is not 0 nothing happnes
        if (grappleCDTimer > 0) return;

        playerMovement.isGrappling = true;

        //shoot a raycast from the player towards the direction the camera is facing and sets the end point if it hits its layer
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

        lr.positionCount = 2;

        //turns the player towards where the grapple is shot
        transform.forward = camForward.forward;
        lr.enabled = true;
        lr.SetPosition(1, grappleHitPoint);
    }

    /// <summary>
    /// handels the grapple points and the physics for pulling you in an arc towards the point
    /// </summary>
    private void ExecuteGrapple()
    {
        playerMovement.freeze = false;
        
        //takes the lowest point as the its own point -1
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        //subtracts the point the grapple hits - the lowest point
        float grapplePointRelativeYPos = grappleHitPoint.y - lowestPoint.y;
        //pulls us towards the lowest point plus the over shoot so you go in an arc form 
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        //if the grapple points y is smaller than 0 so its lower than you the arc is the nrmal arc
        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        playerMovement.JumpToPosition(grappleHitPoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);

    }

    private void StopGrapple()
    {
        playerMovement.freeze = false;
        playerMovement.isGrappling = false;
        grappleCDTimer = grappleCD;
        lr.positionCount = 0;
        lr.enabled = false;
    }

    public void OnGrappleShot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartGrapple();
        }
    }
    #endregion
}
