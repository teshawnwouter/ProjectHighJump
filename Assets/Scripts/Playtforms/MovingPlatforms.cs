using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private PathToNextWaypoint pathToNextWaypoint;

    [SerializeField] private float speed = 1f;

    private int waypointIndex;

    private Transform previousWaypoint, targetedWayPoint;

    private float timeToWaypoint;
    private float elapasedTime;

    private void Start()
    {
        TargetNextWaypoint();
    }

    private void FixedUpdate()
    {
        elapasedTime += Time.deltaTime;

        float elaspasedTimePer = elapasedTime / timeToWaypoint;

        elaspasedTimePer = Mathf.SmoothStep(0, 1, elaspasedTimePer);

        transform.position = Vector3.Lerp(previousWaypoint.position, targetedWayPoint.position, elaspasedTimePer);
        transform.rotation = Quaternion.Lerp(previousWaypoint.rotation, targetedWayPoint.rotation, elaspasedTimePer);

        if (elaspasedTimePer >= 1)
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        previousWaypoint = pathToNextWaypoint.GetWaypoint(waypointIndex);
        waypointIndex = pathToNextWaypoint.GetNextWayPointIndex(waypointIndex);
        targetedWayPoint = pathToNextWaypoint.GetWaypoint(waypointIndex);

        elapasedTime = 0;

        float distanceToWaypoint = Vector3.Distance(previousWaypoint.position, targetedWayPoint.position);

        timeToWaypoint = distanceToWaypoint / speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlatformChild>(out PlatformChild platformChild))
        {
            platformChild.Attach(this.gameObject);
        }
        else
        {
            return;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlatformChild>(out PlatformChild platformChild))
        {
            platformChild.Detach(this.gameObject);
        }
        else
        {
            return;
        }
    }
}
