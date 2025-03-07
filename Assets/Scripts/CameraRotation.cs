using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    public LayerMask collissionLayer;
    private float mincollissionOffsetDist;

    private Vector2 cameVector;
    private Vector3 camposVector;
    private Vector3 cameraVelocity = Vector3.zero;

    public Transform target;
    public Transform camPivot;
    public Transform camTransform;
    private float sense = 0.1f;
    private float defaultPos;
    private float cameraOffset = 0.2f;

    private float camDetectionRadius = 10f;

    private float followSpeed = 0.1f;

    private float lookAngle;
    private float pivitAngle;

    private void Awake()
    {
        target = FindFirstObjectByType<PlayerMovement>().transform;
        camTransform = Camera.main.transform;
        defaultPos = camTransform.localRotation.z;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        cameVector = context.ReadValue<Vector2>();
    }

    public void CamHandeler()
    {
        FollowPlayer();
        CamRotation();
        CamCollissionHandler();
    }
    private void FollowPlayer()
    {
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, target.position, ref cameraVelocity, followSpeed);

        transform.position = targetPos;
    }

    private void CamRotation()
    {
        lookAngle = lookAngle + (cameVector.x * sense);
        pivitAngle = pivitAngle - (cameVector.y * sense);
        pivitAngle = Mathf.Clamp(pivitAngle, -90, 90);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;


        rotation.x = pivitAngle;    
        targetRotation = Quaternion.Euler(rotation);
        transform.localRotation = targetRotation;

    }

    private void CamCollissionHandler()
    {
        float targetPos = defaultPos;

        RaycastHit hit;
        Vector3 direction = camTransform.position - camPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(camTransform.transform.position, camDetectionRadius, direction, out hit, Mathf.Abs(targetPos), collissionLayer))
        {
            Debug.Log("wall");
            float distance = Vector3.Distance(camPivot.position, hit.point);
            targetPos = -(distance - cameraOffset);
        }

        if(Mathf.Abs(targetPos) < mincollissionOffsetDist )
        {
            targetPos = targetPos - mincollissionOffsetDist;
        }

        camposVector.z = Mathf.Lerp(camTransform.localPosition.z, targetPos, 0.2f);

        camTransform.localPosition = camposVector;
    }
}
