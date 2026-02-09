using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class FX_FlameFlicker : MonoBehaviour
{
    [SerializeField]
    private float2 _flickerRange;
    [SerializeField]
    private Light _pointLight;
    private float _tagetIntensity;

    private void Start()
    {
        StartCoroutine(RandomizeFlickerTarget());
    }

    private void Update()
    {
        _pointLight.intensity = Mathf.Lerp(_pointLight.intensity, _tagetIntensity, Time.deltaTime * 5f);
    }

    private IEnumerator RandomizeFlickerTarget()
    {
        while (true)
        {
            _tagetIntensity = UnityEngine.Random.Range(_flickerRange.x, _flickerRange.y);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
