using UnityEngine;

public class RotatePlate : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
        {
            other.transform.parent.SetParent(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
        {
            other.transform.parent.SetParent(null);
        }
    }
 
}
