using UnityEngine;

public class TiltingPlatforms : MonoBehaviour
{
    [Header("check")]
    private bool shouldPlatformRotate;

    [Header("player Tracking")]
    private float playerLenght;
    private Transform player;

    [Header("references")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float MaxTilt = 30f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shouldPlatformRotate = true;
            player = collision.gameObject.GetComponent<Transform>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shouldPlatformRotate = false;
            player  =  null;
        }
    }

    private void Update()
    {
        if (shouldPlatformRotate)
        {
            Vector3 playerPos = transform.InverseTransformPoint(player.position);
            float tiltDirMultiplierX = CalculateTiltDIrX(playerPos);
            float tiltDirMultiplierZ = CalculateTiltDIrZ(playerPos);
            int TiltDirX = playerPos.x < 0 ? 1 : -1;
            int TiltDirZ = playerPos.z < 0 ? 1 : -1;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(MaxTilt * TiltDirX, Vector3.forward), tiltSpeed * Time.deltaTime * tiltDirMultiplierX);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(MaxTilt * TiltDirZ, Vector3.left), tiltSpeed * Time.deltaTime * tiltDirMultiplierZ);

        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), tiltSpeed * Time.deltaTime);
        }
    }


    private float CalculateTiltDIrX(Vector3 playerPosX)
    {
        int TiltDir = playerPosX.x < 0 ? 1 : -1;
        float tiltSpeedMultiplier = Mathf.Abs(Mathf.Clamp((playerPosX.x * 2 / playerLenght) * tiltSpeed, -1, 1));

        return tiltSpeedMultiplier;
    }

    private float CalculateTiltDIrZ(Vector3 playerPosZ)
    {
        int TiltDir = playerPosZ.z < 0 ? 1 : -1;
        float tiltSpeedMultiplier = Mathf.Abs(Mathf.Clamp((playerPosZ.z * 2 / playerLenght) * tiltSpeed, -1, 1));

        return tiltSpeedMultiplier;
    }
}
