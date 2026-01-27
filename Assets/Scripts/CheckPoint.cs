using UnityEngine;
using Game;
public class CheckPoint : MonoBehaviour
{

    public bool isGoalPoint = false;
    Collider col;


    private void Awake()
    {
        col = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.TryGetComponent<CheckManager>(out var check))
        {
            if (isGoalPoint)
            {
                if (check.checkPointList.Count == 0) return;
                check.GoalCheck();
            }
            else
            {
                check.AddCheckPoint(this);

            }
        }

    }

}
