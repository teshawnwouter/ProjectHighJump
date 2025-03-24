using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [Header("input")]
    private Vector2 lookVector;
    [Header("Player")]
    public  GameObject camHolder;

    [Header("sensitivity")]
    [SerializeField] private float sense = 15f;

    [Header("positions")]
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
