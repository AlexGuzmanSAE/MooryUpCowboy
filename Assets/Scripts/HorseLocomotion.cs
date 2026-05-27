using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HorseLocomotion : MonoBehaviour
{
    
    public ReinController reinController;
    public HeadsetRotation headsetRotation;

    [Header("Velocidades del caballo")]
    [Tooltip("Velocidad máxima de movimiento en metros/segundo (galope)")]
    public float maxSpeed = 8f;

    [Tooltip("Velocidad de aceleración (m/s²)")]
    public float acceleration = 6f;

    [Tooltip("Velocidad de frenado cuando no hay input (m/s²)")]
    public float deceleration = 10f;

    [Header("Gravedad")]
    [Tooltip("Fuerza de gravedad (m/s²)")]
    public float gravity = 20f;

    [Tooltip("Nombre del parámetro float de velocidad en el Animator")]
    public string animSpeedParam = "Speed";

    [Tooltip("Nombre del parámetro float de giro en el Animator")]
    public string animTurnParam = "TurnSpeed";

    [Header("Audio (opcional)")]
    [Tooltip("AudioSource para el galope. Se activa/desactiva según velocidad.")]
    public AudioSource gallopAudio;

    [Tooltip("Velocidad normalizada mínima para que suene el galope")]
    [Range(0f, 1f)]
    public float gallopAudioThreshold = 0.1f;

    private CharacterController _charController;
    private float _currentSpeed;
    private float _verticalVelocity;

    [Header("Debug")]
    [Tooltip("Mostrar velocidad y giro en pantalla (Editor)")]
    public bool showDebugGUI = true;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (reinController == null) Debug.LogError("[HorseLocomotion] ReinController no asignado.");
        if (headsetRotation == null) Debug.LogError("[HorseLocomotion] HeadsetRotation no asignado.");
    }

    private void Update()
    {
        HandleGravity();
        HandleRotation();
        HandleMovement();
        UpdateAudio();
    }

    private void HandleGravity()
    {
        if (_charController.isGrounded)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            _verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    private void HandleRotation()
    {
        if (headsetRotation == null) return;

        float turnThisFrame = headsetRotation.turnDegreesPerSecond * Time.deltaTime;
        transform.Rotate(Vector3.up, turnThisFrame, Space.World);
    }

    private void HandleMovement()
    {
        if (reinController == null) return;

        float targetSpeed = reinController.normalizedSpeed * maxSpeed;

        // Aceleracion / frenado suaves
        if (targetSpeed > _currentSpeed)
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, deceleration * Time.deltaTime);
        }

        // Vector de movimiento en coordenadas del mundo
        Vector3 forward = transform.forward * _currentSpeed;
        Vector3 motion = new Vector3(forward.x, _verticalVelocity, forward.z);

        _charController.Move(motion * Time.deltaTime);
    }

    private void UpdateAudio()
    {
        if (gallopAudio == null) return;

        bool shouldPlay = reinController != null && reinController.normalizedSpeed > gallopAudioThreshold;
        if (shouldPlay && !gallopAudio.isPlaying)
        {
            gallopAudio.Play();
        }
        else if (!shouldPlay && gallopAudio.isPlaying)
        {
            gallopAudio.Stop();
        }

        if (shouldPlay)
        {
            gallopAudio.pitch = Mathf.Lerp(0.8f, 1.3f, reinController.normalizedSpeed);
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (!showDebugGUI) return;

        GUIStyle style = new GUIStyle(GUI.skin.label) { fontSize = 16 };
        float spd = reinController != null ? reinController.normalizedSpeed : 0f;
        float turn = headsetRotation != null ? headsetRotation.turnDegreesPerSecond : 0f;

        GUI.Label(new Rect(10, 10, 400, 25), $"Velocidad normalizada: {spd:F2}", style);
        GUI.Label(new Rect(10, 35, 400, 25), $"Velocidad real: {_currentSpeed:F2} m/s", style);
        GUI.Label(new Rect(10, 60, 400, 25), $"Giro: {turn:F1} °/s", style);
        GUI.Label(new Rect(10, 85, 400, 25), $"Yaw caballo: {transform.eulerAngles.y:F1}°", style);
    }
#endif
}