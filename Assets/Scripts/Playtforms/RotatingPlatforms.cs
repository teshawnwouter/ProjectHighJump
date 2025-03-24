using UnityEngine;

public class RotatingPlatforms : MonoBehaviour
{
    [Header("cooldown and way time")]
    [SerializeField] private float rotateCooldown;
    private float rotateTimer;

    [Header("Rotation")]
    private Quaternion targetRot;
    private float rotationSpeed = 10f;

    [Header("Stop Point")]
    private float stopTreshold = 3f;

    private void Start()
    {
        targetRot = transform.rotation;
    }
    private void Update()
    {
        AllowToRotate();
        if (rotateTimer > 0)
        {
            rotateTimer -= Time.deltaTime;
        }

        if (rotateTimer <= 0)
        {
            RotatingPlatform();
        }
    }


    private void RotatingPlatform()
    {
        float rotAngel = rotationSpeed * Time.deltaTime;
        transform.Rotate(rotAngel , 0, 0);
    }

    private void AllowToRotate()
    {
        float rotDiffrence = Mathf.Abs(Quaternion.Angle(transform.rotation, targetRot));
        if (rotDiffrence < stopTreshold)
        {
            transform.rotation = targetRot;
            rotateTimer = rotateCooldown;
            targetRot *= Quaternion.Euler(180, 0, 0);
        }
    }
}
