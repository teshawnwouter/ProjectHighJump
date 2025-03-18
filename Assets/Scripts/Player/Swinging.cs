using UnityEngine;
using UnityEngine.InputSystem;

public class Swinging : MonoBehaviour
{
    [Header("Swing")]
    private float maxSwingDist = 25f;
    private Vector3 swingPoint;
    public SpringJoint joint;
    private Vector3 currentGrapplePos;

    [Header("Refrerances")]
    public LineRenderer lineRenderer;
    public Transform SwingHookPoint;
    public LayerMask SwingAble;
    private PlayerMovement pm;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        DrawRope();
    }
    private void StartSWing()
    {
        RaycastHit hit;

        Vector2 centerofScreen = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(centerofScreen);

        if (Physics.Raycast(ray, out hit, maxSwingDist, SwingAble))
        {
            pm.isSwining = true;
            swingPoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distFromPoint * 0.8f;
            joint.minDistance = distFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;
        }

        lineRenderer.enabled = true;
        currentGrapplePos = transform.position;
    }

    public void StopSwing()
    {
        pm.isSwining = false;
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    private void DrawRope()
    {
        if (!joint) return;

        currentGrapplePos = Vector3.Lerp(currentGrapplePos, swingPoint, Time.deltaTime * 8f);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, swingPoint);
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
