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

    public void AddCheckPoint(CheckPoint point)
    {
        if(!checkPointList.Contains(point))
        {
            checkPointList.Add(point); 
        }
         
    }

    IEnumerator DelayStop()
    {
        yield return new WaitForSeconds(1);
        gameObject.TryGetComponent<CarAIControl>(out var aiControl);
        aiControl.StopDrive();
    }
}
