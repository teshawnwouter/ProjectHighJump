using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Vector2 lookVector;

    private GameObject camHolder;

    public Transform camTransform;
    private RaycastHit hit;
    private Vector3 camOffset;

    [SerializeField] private float sense = 15f;

    [SerializeField] private float topCamPos, bottomCamPos;

    private float YAngle, XAngle;

    private void Awake()
    {
        camHolder = transform.parent.gameObject;
       
    }
    private void Start()
    {
        camOffset.y = transform.parent.localPosition.y;
        camOffset.z = transform.localPosition.z;
        camOffset.x = transform.localPosition.x;
    }

    private void Update()
    {
        lookVector = lookVector * sense * Time.deltaTime;

        XAngle += lookVector.x;
        YAngle -= lookVector.y;
        YAngle = Mathf.Clamp(YAngle, bottomCamPos, topCamPos);
        camHolder.transform.rotation = Quaternion.Euler(YAngle, XAngle, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            lookVector = context.ReadValue<Vector2>();
        }
    }

}
