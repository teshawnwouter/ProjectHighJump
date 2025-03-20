using UnityEngine;

public class PathToNextWaypoint : MonoBehaviour
{
    public Transform GetWaypoint(int WayPoint)
    {
        return transform.GetChild(WayPoint);
    }

    public int GetNextWayPointIndex(int currentWaypoint)
    {
        int nextWaypoint = currentWaypoint + 1;

        if (nextWaypoint == transform.childCount)
        {
            nextWaypoint = 0;
        }

        return nextWaypoint;
    }
}

