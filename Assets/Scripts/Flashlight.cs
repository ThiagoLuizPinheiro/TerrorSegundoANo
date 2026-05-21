using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light _light;
    private float _originalIntensity;
    [SerializeField]private float _intensityDecreaseRate = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _light = GetComponent<Light>();
        _originalIntensity = _light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (_light.intensity <= 0)
            return;

        _light.intensity -= Time.deltaTime * _intensityDecreaseRate;
    }
}
