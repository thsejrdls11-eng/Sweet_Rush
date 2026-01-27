using UnityEngine;
using UnityEngine.Timeline;
using Unity.Mathematics;

public class CameraController : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offset;
    public quaternion offsetRotation;
    public float smoothSpeed = 0.125f;
    

  

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.TransformPoint(offset),Time.deltaTime* smoothSpeed);
        //transform.LookAt(targetTransform);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation * offsetRotation, Time.deltaTime*smoothSpeed);
        
    }
}
