using UnityEngine;

public class CamCollision : MonoBehaviour
{
    [Header("Distance")]
    private float minDist = 1f;
    private float maxDist = 4;
    private float distance;

    [Header("posistion")]
    private float smooth = 10f;

    [Header("direction")]
    private Vector3 dollyDir;


    private void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update()
    {
        Vector3 desiredCam = transform.parent.TransformPoint(dollyDir * maxDist);
        RaycastHit hit;

        if(Physics.Linecast(transform.parent.position,desiredCam,out hit))   
        {
            distance = Mathf.Clamp(hit.distance * 0.9f, minDist, maxDist);
        }
        else
        {
            distance = maxDist;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition,dollyDir * distance, Time.deltaTime * smooth);
    }
}
