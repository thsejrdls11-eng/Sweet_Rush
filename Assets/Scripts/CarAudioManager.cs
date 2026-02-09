using UnityEngine;

public class CarAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _engineAudioSource;
    [SerializeField]
    private CarController2 _carController;

    [SerializeField]
    private AnimationCurve _engineAudioCurve;

    private float _maxSpd;


    //[SerializeField]
    //private float _TEST_PITCH;

    private void Start()
    {
        _maxSpd = _carController.maxSpd;
    }

    private void Update()
    {
        // _engineAudioSource.pitch = _engineAudioCurve.Evaluate(_TEST_PITCH);
        _engineAudioSource.pitch = _engineAudioCurve.Evaluate(_carController.currentSpeed / _maxSpd);

    }
}
