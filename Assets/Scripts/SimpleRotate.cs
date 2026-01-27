using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public float rotateSpd = 5f;
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpd);
    }
}
