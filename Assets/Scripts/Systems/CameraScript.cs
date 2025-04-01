using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Transform Objects")]
    public Transform camtarget;
    [SerializeField] private Transform oriantation;
    [SerializeField] private Transform player;

    private void Update()
    {
        Vector3 camDir = camtarget.position - new Vector3(transform.position.x, camtarget.position.y, transform.position.z);
        oriantation.forward = camDir.normalized;

        player.forward = camDir.normalized;
    }
}
