using Game;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Vehicles.Car;

public class RingOut : MonoBehaviour
{
    public WaypointCircuit waypoints;

    private void Awake()
    {
        waypoints = FindFirstObjectByType<WaypointCircuit>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<Rigidbody>(out var rb))
        {
            RePosition(rb);


        }
    }

    public void RePosition(Rigidbody rb)
    {
        Transform nearestWaypoint;
        if (rb.GetComponent<CheckManager>().checkPointList.Count > 0)
        {
            nearestWaypoint = rb.GetComponent<CheckManager>().checkPointList.Last<CheckPoint>().transform;
        }
        else
        {
            nearestWaypoint = waypoints.Waypoints[0];
        }
        if (nearestWaypoint != null)
        {
            // 리지드바디가 있으면 물리 기반 이동
            rb.linearVelocity = Vector3.zero; // 현재 속도 초기화
            rb.angularVelocity = Vector3.zero; // 회전 속도 초기화
            rb.transform.rotation = nearestWaypoint.rotation; //웨이포인트를 미리 코스에 맞춰 회전시켜놓을것
                                                              // 위치 이동
            rb.transform.position = nearestWaypoint.position;
            if (rb.TryGetComponent<WaypointProgressTracker>(out var npcCar)) npcCar.Reset();
        }
    }

    public void BtnRePos()
    {
        RePosition(MainManager.instance.player.GetComponent<Rigidbody>());
    }
        
    private Transform FindNearestWaypoint(Vector3 position)
    {
        if (waypoints == null || waypoints.waypointList == null || waypoints.waypointList.items.Length == 0)
        {
            Debug.LogWarning("Waypoints not configured!");
            return null;
        }

        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform waypoint in waypoints.waypointList.items)
        {
            if (waypoint == null) continue;

            float distance = Vector3.Distance(position, waypoint.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = waypoint;
            }
        }

        return nearest;
    }
}
