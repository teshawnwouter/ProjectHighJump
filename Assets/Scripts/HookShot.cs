using UnityEngine;
using UnityEngine.InputSystem;

public class HookShot : MonoBehaviour
{
    LayerMask grappleAbleLayer;
    public float maxDistance;
    public float grappleDelay;
    public float grapplingCdTimer;
    public float grapplingCd;
    public Vector3 grapplePoint;
    public float grappleForce;
    public bool hookShooting;
    public float lastCheck;
    public float oldValue;
    public bool allowDismount;

    public Transform grappleShootPoint;


    private void Start()
    {
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
        if(Physics.Raycast(transform.position,transform.forward,out RaycastHit hitInfo, maxDistance, grappleAbleLayer))
        {
            grapplePoint = hitInfo.point;
            CheckOldValue(1000f);
        }
    }

    private void ShootGrapple()
    {

    }


    private void AllowJump()
    {

    }

    private void StopGrapple()
    {

    }

    private void CheckOldValue(float value)
    {

    }

    public void OnGrappleShoot(InputAction.CallbackContext context)
    {

    }
}
