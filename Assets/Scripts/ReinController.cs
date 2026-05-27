using UnityEngine;
using UnityEngine.XR;

public class ReinController : MonoBehaviour
{
    [Header("Referencias XR")]
    
    public Transform leftController;
    public Transform rightController;
    

    [Header("Parametros de riendas")]
    public float minReinAmplitude = 0.08f;
    public float minCycleFrequency = 0.8f;
    public float maxCycleFrequency = 2.5f;

    public float idleTimeout = 0.6f;

  
    [HideInInspector] public float normalizedSpeed;

    private float _leftPrevY, _rightPrevY;
    private float _leftPeak, _rightPeak;
    private float _lastCycleTime;
    private float _cycleFrequency;
    private bool _leftGoingUp, _rightGoingUp;

    private void Start()
    {
        if (leftController == null) Debug.LogWarning("[ReinController] leftController no asignado.");
        if (rightController == null) Debug.LogWarning("[ReinController] rightController no asignado.");

        _leftPrevY = GetLeftY();
        _rightPrevY = GetRightY();
        _lastCycleTime = Time.time;
    }

    private void Update()
    {
        float leftY = GetLeftY();
        float rightY = GetRightY();

        bool leftUp = leftY > _leftPrevY;
        bool rightUp = rightY > _rightPrevY;

        _leftPeak = _leftGoingUp ? Mathf.Max(_leftPeak, leftY - _leftPrevY) : _leftPeak;
        _rightPeak = _rightGoingUp ? Mathf.Max(_rightPeak, rightY - _rightPrevY) : _rightPeak;

        
        if (_leftGoingUp && !leftUp && _leftPeak >= minReinAmplitude)
        {
            RegisterCycle();
            _leftPeak = 0f;
        }

        _leftGoingUp = leftUp;
        _rightGoingUp = rightUp;
        _leftPrevY = leftY;
        _rightPrevY = rightY;

        
        float timeSinceLastCycle = Time.time - _lastCycleTime;
        if (timeSinceLastCycle > idleTimeout)
        {
            _cycleFrequency = Mathf.MoveTowards(_cycleFrequency, 0f, Time.deltaTime * 2f);
        }

        normalizedSpeed = Mathf.InverseLerp(minCycleFrequency, maxCycleFrequency, _cycleFrequency);
        normalizedSpeed = Mathf.Clamp01(normalizedSpeed);
    }

    private void RegisterCycle()
    {
        float now = Time.time;
        float interval = now - _lastCycleTime;
        if (interval > 0.05f)
        {
            _cycleFrequency = 1f / interval;
        }
        _lastCycleTime = now;
    }

    private float GetLeftY() => leftController != null ? leftController.position.y : 0f;
    private float GetRightY() => rightController != null ? rightController.position.y : 0f;
}