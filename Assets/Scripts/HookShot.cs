using UnityEngine;
using UnityEngine.InputSystem;

public class HookShot : MonoBehaviour
{
    public Camera cam;
    public LayerMask grappleAbleLayer;
    public float maxDistance;
    public float grappleDelay;
    public float grapplingCdTimer;
    public float grapplingCd;
    public Vector3 grapplePoint;
    public float grappleForce;
    public bool hookShooting;
    public float lastCheck;
    public float oldValue;

    public Transform grappleShootPoint;

    private PlayerMovement playerMovement;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if(grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }
        if(Vector3.Distance(transform.position, grapplePoint) < 2 && hookShooting)
        {
            StopGrapple();
        }
        if(Vector3.Distance(transform.position,grapplePoint) > oldValue)
        {
            StopGrapple();
        }
        if(hookShooting)
        {
            lastCheck -= Time.deltaTime;
        }
        if (hookShooting && lastCheck < 0)
        {
            lastCheck = 0.1f;
            CheckOldValue(0);
        }
    }

    private void HookPoint()
    {
        Vector2 middleOfTheScreen = new Vector2(Screen.width/2, Screen.height/2);
        Ray ray = Camera.main.ScreenPointToRay(middleOfTheScreen);

        if(Physics.Raycast(ray,out RaycastHit hitInfo, maxDistance, grappleAbleLayer))
        {

            Vector3 direction = hitInfo.point - grapplePoint;
            Invoke(nameof(ExecuteGrapple), grappleDelay);
            grapplePoint = hitInfo.point;
            CheckOldValue(1000f);
        }
        else
        {
            grapplePoint = ray.GetPoint(maxDistance);
        }
    }

    private void ExecuteGrapple()
    {
        playerMovement.isGrappling = true;
        playerMovement.acitvateGrapple = true;
        hookShooting = true;
        rb.useGravity = false;

        rb.AddForce(rb.transform.forward * grappleForce, ForceMode.Force);  
    }

    private void StopGrapple()
    {
        playerMovement.isGrappling = false;
        playerMovement.acitvateGrapple = false;
        hookShooting = false;
        rb.useGravity = true;
        grapplingCdTimer = grapplingCd;
    }

    private void CheckOldValue(float value)
    {
        oldValue = Vector3.Distance(transform.position, grapplePoint) + value;
    }

    public void OnGrappleShoot(InputAction.CallbackContext context)
    {
        if (context.performed && grapplingCd > 0)
        {
            HookPoint();
        }
    }
}
