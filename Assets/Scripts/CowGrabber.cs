using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.DebugUI.Table;

public class CowGrabber : MonoBehaviour
{
    [Header("Raycast")]
    public float rayDistance = 10f;
    public LayerMask cowLayer;
    public Transform rayOrigin; // punta del controlador

    [Header("Hold Settings")]
    public float requiredHoldTime = 1.5f; // segundos para cargar

    [Header("Input Action")]
    public InputActionReference triggerAction;

    [Header("Visual Feedback")]
    public LineRenderer lineRenderer;
    public GameObject chargeIndicator; // UI de carga

    private float holdTimer = 0f;
    private bool isTriggerHeld = false;
    private bool isCharged = false;
    private Cow_Info targetCow = null;

    private Vector3[] renderPos;

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
        holdTimer = 0f;
        isCharged = false;
    }

    void OnTriggerReleased(InputAction.CallbackContext ctx)
    {
        isTriggerHeld = false;

        // Solo agarra si estaba cargado Y apuntando a una vaca
        if (isCharged && targetCow != null)
        {
            targetCow.GetGrabbed(transform);
        }

        // Reset
        holdTimer = 0f;
        isCharged = false;
        targetCow = null;
        UpdateLineRenderer(false, Vector3.zero);
        if (chargeIndicator) chargeIndicator.SetActive(false);
    }

    void Update()
    {
        if (!isTriggerHeld) return;


        // Raycast
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward * 50);
        bool hitCow = Physics.Raycast(ray, out RaycastHit hit, rayDistance, cowLayer);


        if (hitCow)
        {
            renderPos[0] = transform.position;
            renderPos[1] = hit.collider.gameObject.transform.position;
            lineRenderer.SetPositions(renderPos);
            targetCow = hit.collider.GetComponent<Cow_Info>();
            UpdateLineRenderer(true, hit.point);

            // Acumular tiempo de carga
            holdTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(holdTimer / requiredHoldTime);

            if (chargeIndicator)
            {
                chargeIndicator.SetActive(true);
            }

            if (holdTimer >= requiredHoldTime && !isCharged)
            {
                isCharged = true;
                // Feedback haptico al completar carga
                TriggerHaptic(0.8f, 0.2f);
                Debug.Log("¡Cargado! Suelta para agarrar la vaca.");
            }
        }
        else
        {
            // Perdiste el objetivo
            targetCow = null;
            holdTimer = Mathf.Max(0, holdTimer - Time.deltaTime * 2f); // se descarga más rápido
            UpdateLineRenderer(true, rayOrigin.position + rayOrigin.forward * rayDistance);
            if (chargeIndicator) chargeIndicator.SetActive(false);
        }
    }

    void UpdateLineRenderer(bool active, Vector3 endPoint)
    {
        //if (!lineRenderer) return;
        //lineRenderer.enabled = active;
        //if (active)
        //{
        //    lineRenderer.SetPosition(0, rayOrigin.position);
        //    lineRenderer.SetPosition(1, endPoint);
        //}
    }

    void TriggerHaptic(float amplitude, float duration)
    {
        // XR Interaction Toolkit haptics
        var device = GetComponent<XRBaseController>();
        if (device != null)
            device.SendHapticImpulse(amplitude, duration);
    }
}