using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField]
    private Transform _respawnPoint;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(ResetCoroutne());
    }


    private IEnumerator ResetCoroutne()
    {
        while(true)
        {
            if (transform.position.y < -20f)
            {
                transform.position = _respawnPoint.position;
                _rigidbody.linearVelocity = Vector3.zero;

                Debug.Log("Respawned");
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
