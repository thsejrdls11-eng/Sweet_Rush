using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointsManager : MonoBehaviour
{
    void Awake()
    {
        int wp = transform.childCount;

        if(wp == 0)
        {
            Debug.Log("No Waypoint was found");
            return;
        }

        for(int i = 0; i < wp; i++)
        {
            Transform w = transform.GetChild(i);
            w.name = "Waypoint" + i.ToString("D1");
        }
    }
}
