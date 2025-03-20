using UnityEngine;

public class RotatingPlatforms : MonoBehaviour
{
    [SerializeField] private bool isRotating;

    [SerializeField] private float rotateCooldown;


    [SerializeField] private float rotationSpeed = 2f;

    private void Start()
    {
        InvokeRepeating(nameof(AllowToRotate), rotateCooldown, 6f);
    }

    private void Update()
    {
        if (isRotating)
        {
            RotatingPlatform();
        }
    }


    private void RotatingPlatform()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
    }

    private void AllowToRotate()
    {
        if (isRotating)
        {
            isRotating = false;
        }
        else
        {
            isRotating = true;
        }
    }
}
