using UnityEngine;

public class HeadsetRotation : MonoBehaviour
{
    [Header("Referencias")]
    public Transform hmdTransform;

    [Header("Zona muerta y sensibilidad")]
    [Range(0f, 30f)]
    public float deadZoneDegrees = 15f;

    [Range(30f, 90f)]
    public float maxHeadAngle = 60f;

    public float maxTurnSpeed = 90f;

    [Range(0f, 0.95f)]
    public float smoothing = 0.15f;

    [HideInInspector] public float turnDegreesPerSecond;

    private float _referenceYaw;  
    private float _smoothedTurn;

    private void Start()
    {
        if (hmdTransform == null)
        {
            hmdTransform = Camera.main != null ? Camera.main.transform : transform;
            Debug.LogWarning("[HeadsetRotation] hmdTransform no asignado, usando Camera.main.");
        }
        RecalibrateForward();
    }

    private void Update()
    {
        float currentYaw = hmdTransform.eulerAngles.y;
        float deltaYaw = Mathf.DeltaAngle(_referenceYaw, currentYaw);

        float absDelta = Mathf.Abs(deltaYaw);
        float activeDelta = 0f;
        if (absDelta > deadZoneDegrees)
        {
            activeDelta = Mathf.Sign(deltaYaw) *
                           Mathf.InverseLerp(deadZoneDegrees, maxHeadAngle, absDelta) *
                           maxTurnSpeed;
        }

        _smoothedTurn = Mathf.Lerp(activeDelta, _smoothedTurn, smoothing);
        turnDegreesPerSecond = _smoothedTurn;
    }

    public void RecalibrateForward()
    {
        if (hmdTransform != null)
        {
            _referenceYaw = hmdTransform.eulerAngles.y;
            Debug.Log($"[HeadsetRotation] Forward recalibrado: {_referenceYaw:F1}°");
        }
    }
}