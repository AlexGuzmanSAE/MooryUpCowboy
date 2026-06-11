using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CowGrabber : MonoBehaviour
{
    [Header("VR Setup")]
    public Transform vrCamera;
    public Transform rigtHand;

    [Header("Raycast")]
    public float rayDistance = 10f;
    public LayerMask cowLayer;
    public Transform rayOrigin;

    [Header("Input Action")]
    public InputActionReference triggerAction;

    [Header("Visual Feedback")]
    public LineRenderer lineRenderer;

    private bool isTriggerHeld = false;
    private COW_IA grabbedCow = null;

    public HapticImpulsePlayer hapticPlayer;

    [Header("Sonidos")]
    public SoundData controlTrigger_SD;
    public SoundData cowHit_SD;
    public SoundData scoreSD;

    void OnEnable()
    {
        triggerAction.action.started += OnTriggerPressed;
        triggerAction.action.canceled += OnTriggerReleased;
        triggerAction.action.Enable();
    }

    void OnDisable()
    {
        triggerAction.action.started -= OnTriggerPressed;
        triggerAction.action.canceled -= OnTriggerReleased;
        triggerAction.action.Disable();
    }

    private void Start()
    {
        if (lineRenderer == null)
            lineRenderer = gameObject.GetComponent<LineRenderer>();

        UpdateLineRenderer(false, Vector3.zero);
    }

    void OnTriggerPressed(InputAction.CallbackContext ctx)
    {
        isTriggerHeld = true;
        SoundManager.Instance.CreateSound().WithSoundData(controlTrigger_SD).WithRandomPitch().Play();
    }

    void OnTriggerReleased(InputAction.CallbackContext ctx)
    {
        isTriggerHeld = false;

        if (grabbedCow != null)
        {
            grabbedCow.transform.SetParent(null);
            grabbedCow.currentCowState = COW_IA.cowStates.Idle;
            grabbedCow = null;
        }

        UpdateLineRenderer(false, Vector3.zero);
    }

    void Update()
    {
        if (grabbedCow != null)
        {
            if (vrCamera == null)
                vrCamera = FindAnyObjectByType<Camera>().transform;

            Vector3 dirToHand = (rayOrigin.position - vrCamera.position).normalized;
            float dot = Vector3.Dot(Vector3.up, rigtHand.forward);

            if (dot > 0.9)
            {
                TriggerHaptic(1f, 0.5f);
                grabbedCow.DestroyCow();
                grabbedCow = null;
                UpdateLineRenderer(false, Vector3.zero);
                SoundManager.Instance.CreateSound().WithSoundData(scoreSD).Play();
                return;
            }

            if (grabbedCow != null)
            {
                UpdateLineRenderer(true, grabbedCow.transform.position);
            }
        }
        else if (isTriggerHeld)
        {
            Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
            bool hitCow = Physics.Raycast(ray, out RaycastHit hit, rayDistance, cowLayer);

            if (hitCow)
            {
                UpdateLineRenderer(true, hit.point);

                COW_IA targetCow = hit.collider.GetComponent<COW_IA>();

                  if (targetCow != null && targetCow.currentCowState != COW_IA.cowStates.Grabbed)
                {
                    SoundManager.Instance.CreateSound().WithSoundData(cowHit_SD).WithRandomPitch().Play();
                    grabbedCow = targetCow;
                    grabbedCow.GetGrabbed(rayOrigin);
                    TriggerHaptic(0.5f, 0.1f);
                }
            }
            else
            {
                UpdateLineRenderer(true, rayOrigin.position + rayOrigin.forward * rayDistance);
            }
        }
        else
        {
            UpdateLineRenderer(false, Vector3.zero);
        }
    }

    void UpdateLineRenderer(bool active, Vector3 endPoint)
    {
        if (!lineRenderer) return;

        lineRenderer.enabled = active;
        if (active)
        {
            lineRenderer.SetPosition(0, rayOrigin.position);
            lineRenderer.SetPosition(1, endPoint);
        }
    }

    void TriggerHaptic(float amplitude, float duration)
    {
        if (hapticPlayer == null)
            hapticPlayer = GetComponent<HapticImpulsePlayer>();

        if (hapticPlayer != null)
            hapticPlayer.SendHapticImpulse(amplitude, duration);
    }
}