using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Vehicles.Car;
namespace Game
{
    public class MainManager : MonoBehaviour
    {
        //public CameraController cam;
        public CinemachineCamera cam;
        public UIManager uiManager;
        public WaypointCircuit waypointCircuit;
        public static MainManager instance;
        int selectIndex = 0;
        public List<GameObject> carList = new List<GameObject>();
        public CheckManager player;
        public int maxLapCount = 3;
        public int LapCount = 0;
        public int maxWaypoint = 0;
        public int currentWaypoint = 0;
        public List<CheckManager> goalList = new List<CheckManager>();
        public List<CheckManager> allCar = new List<CheckManager>();
        public int rank = 1;
        public bool isStart = true;




        public float timer = 0;
        private void Awake()
        {
            instance = this;
            selectIndex = PlayerData.selectIndex;
            for (int i = 0; i < carList.Count; i++)
            {
                if (i == selectIndex)
                {
                    carList[i].SetActive(true);
                    cam.Target.TrackingTarget = carList[i].transform;
                    player = carList[i].GetComponent<CheckManager>();
                }
                else carList[i].SetActive(false);
            }


        }
        private void Start()
        {
            maxWaypoint = waypointCircuit.Waypoints.Length - 1;
            allCar = FindObjectsByType<CheckManager>(FindObjectsSortMode.None).ToList<CheckManager>();
            foreach (var a in allCar)
            {
                if (a.CompareTag("Player")) return;
                a.GetComponent<CarAIControl>().StopDrive();
            }

        }

        private void FixedUpdate()
        {
            if (isStart) CalculateRank();
        }

        public void CalculateRank()
        {
            rank = 1;
            foreach (var other in allCar)
            {
                if (other == this)
                    continue;

                if (other.labCount > player.labCount)
                {
                    rank++;
                }
                else if (other.labCount == player.labCount)
                {
                    if (other.checkPointList.Count > player.checkPointList.Count)
                    {
                        rank++;
                    }
                }
            }
            uiManager.UpdateRank();

        }

        public void StartRace()
        {
            isStart = true;

            foreach (var a in allCar)
            {
                if (!a.CompareTag("Player"))
                    a.GetComponent<CarAIControl>().StartDrive();
 
            }
            StartCoroutine(StartTimer());
        }

        public void EndRace()
        {
            isStart = false;

            foreach (var a in allCar)
            {
                if (!a.CompareTag("Player"))
                {
                    a.GetComponent<CarAIControl>().StopDrive();
                    if (!goalList.Contains(a))
                        goalList.Add(a);
                }
                else
                {
                    a.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                }
            }

            uiManager.ResultOpen();
        }

        IEnumerator StartTimer()
        {
            timer = 0;
            while (isStart)
            {
                timer += Time.deltaTime;
                uiManager.UpdateTime();
                yield return null;
            }
        }


        public void EXITbutton()
        {
            SceneManager.LoadScene(0);
        }


    }
}
