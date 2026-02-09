using UnityEngine;

public class MufflerBoostManager : MonoBehaviour
{
    [SerializeField]
    private CarController2 _carController;
    [SerializeField]
    private ParticleSystem _particleSystem;
    [SerializeField]
    private Vector2 _velocity;

    private void Update()
    {
        var emission = _particleSystem.emission;
        emission.rateOverTime = Mathf.Lerp(_velocity.x, _velocity.y, _carController.currentSpeed / _carController.maxSpd);
    }
}
