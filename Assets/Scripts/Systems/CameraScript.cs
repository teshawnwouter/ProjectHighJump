using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Vector2 lookVector;

    public  GameObject camHolder;

    private RaycastHit hit;
    public Vector3 camOffset;
    private float camFollowSpeed = 120f;

    [SerializeField] private float sense = 15f;

    [SerializeField] private float topCamPos, bottomCamPos;

    private float YAngle, XAngle;

    
  

    private void Update()
    {
        lookVector = lookVector * sense * Time.deltaTime;
        XAngle += lookVector.x;
        YAngle -= lookVector.y;
        YAngle = Mathf.Clamp(YAngle, bottomCamPos, topCamPos);
        transform.rotation = Quaternion.Euler(YAngle, XAngle, 0);
    }

    private void LateUpdate()
    {
        CamFollow();
    }

    private void CamFollow()
    {
        transform.position = camHolder.transform.position;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            lookVector = context.ReadValue<Vector2>();
        }
    }

}
