using UnityEngine;

public class HeadsetRotation : MonoBehaviour
{
    [Header("Referencias")]
    public Transform hmdTransform;

    [Header("Zona muerta y sensibilidad")]
    [Range(0f, 30f)]
    public float deadZoneDegrees = 5f;

    [Range(30f, 90f)]
    public float maxHeadAngle = 60f;

    public float maxTurnSpeed = 90f;

    [Range(0f, 0.95f)]
    public float smoothing = 0.15f;

    [HideInInspector] public float turnDegreesPerSecond;

    private float _referenceRoll;  
    private float _smoothedTurn;

    private void Start()
    {
        if (hmdTransform == null)
        {
            hmdTransform = Camera.main != null ? Camera.main.transform : transform;
            
        }
        RecalibrateForward();
    }

    private void Update()
    {
        float currentRoll = hmdTransform.eulerAngles.z;
        float deltaRoll = Mathf.DeltaAngle(_referenceRoll, currentRoll);

        float absDelta = Mathf.Abs(deltaRoll);
        float activeDelta = 0f;
        if (absDelta > deadZoneDegrees)
        {
            activeDelta = Mathf.Sign(deltaRoll) *
                           Mathf.InverseLerp(deadZoneDegrees, maxHeadAngle, absDelta) *
                           maxTurnSpeed;
        }
        _smoothedTurn = Mathf.Lerp(activeDelta, _smoothedTurn, smoothing);
        turnDegreesPerSecond = _smoothedTurn;
        Debug.Log("SmoothedTurn = " + _smoothedTurn + " current roll = " + currentRoll);
    }

    public void RecalibrateForward()
    {
        if (hmdTransform != null)
        {
            _referenceRoll = hmdTransform.eulerAngles.z;
            Debug.Log($"[HeadsetRotation] Forward recalibrado: {_referenceRoll:F1}°");
        }
    }
}