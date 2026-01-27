using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using Game;
public class CheckManager : MonoBehaviour
{
    public List<CheckPoint> checkPointList = new List<CheckPoint>();
    public int labCount = 0;
    public float time = 0;
    
    public void GoalCheck()
    {
        if (checkPointList.Count >= MainManager.instance.maxWaypoint)
        {
            checkPointList.Clear();
            labCount++;

            if (labCount == MainManager.instance.maxLapCount)
            {
                time = MainManager.instance.timer;
                if(!MainManager.instance.goalList.Contains(this))
                    MainManager.instance.goalList.Add(this);
                if(gameObject.CompareTag("Player"))
                {
                     MainManager.instance.EndRace();                    
                }
                else
                {
                    StartCoroutine(DelayStop());
                }
            }
            if (gameObject.CompareTag("Player")) MainManager.instance.uiManager.UpdateLap();
        }
        else
        {
            //∫Œ¡§?
            return;
        }
    }

    public void AddCheckPoint(CheckPoint point)
    {
        if(!checkPointList.Contains(point))
        {
            checkPointList.Add(point); 
        }
         
    }

    IEnumerator DelayStop()
    {
        yield return new WaitForSeconds(3);
        gameObject.TryGetComponent<CarAIControl>(out var aiControl);
        aiControl.StopDrive();
    }
}
