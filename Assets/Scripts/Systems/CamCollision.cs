using UnityEngine;

public class CamCollision : MonoBehaviour
{
    private float minDist = 1f;
    private float maxDist = 4;
    private Vector3 dollyDir;
    private Vector3 adjustedDollyDir;
    private float distance;


    private void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update()
    {
        Vector3 desiredCam = transform.parent.TransformPoint(dollyDir * maxDist);
        RaycastHit hit;

        //if(Physics.Raycast(transform.parent.position,desiredCam,))
    }
}
