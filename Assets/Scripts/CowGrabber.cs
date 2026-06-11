using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CowGrabber : MonoBehaviour
{
    [Header("VR Setup")]
    public Transform vrCamera; // OBLIGATORIO: Arrastra tu Main Camera de XR aquí
    public Transform rigtHand;

    [Header("Raycast")]
    public float rayDistance = 10f;
    public LayerMask cowLayer;
    public Transform rayOrigin; // Punta del controlador

    [Header("Input Action")]
    public InputActionReference triggerAction;

    [Header("Visual Feedback")]
    public LineRenderer lineRenderer;

    private bool isTriggerHeld = false;
    private COW_IA grabbedCow = null; // Cambiado a COW_IA
    private Vector3[] renderPos = new Vector3[2];

    public HapticImpulsePlayer hapticPlayer;

    //Sonidos
    public SoundData controlTrigger_SD;
    public SoundData cowHit_SD;
    public SoundData scoreSD;
    //
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
    private void Start()
    {
        if(lineRenderer == null)
            lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (grabbedCow != null)
        {
            if(vrCamera == null)
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
        }

        if (isTriggerHeld && grabbedCow == null)
        {
            Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
            bool hitCow = Physics.Raycast(ray, out RaycastHit hit, rayDistance, cowLayer);

            if (hitCow)
            {
                SoundManager.Instance.CreateSound().WithSoundData(cowHit_SD).WithRandomPitch().Play();
                COW_IA targetCow = hit.collider.GetComponent<COW_IA>();

                if (targetCow != null && targetCow.currentCowState != COW_IA.cowStates.Grabbed)
                {
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

        if (grabbedCow != null)
        {
            renderPos[0] = rayOrigin.position;
            renderPos[1] = grabbedCow.transform.position;
            lineRenderer.enabled = true;
            lineRenderer.SetPositions(renderPos);
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
        hapticPlayer = GetComponent<HapticImpulsePlayer>();
        hapticPlayer.SendHapticImpulse(0.5f, 0.2f);
    }
}