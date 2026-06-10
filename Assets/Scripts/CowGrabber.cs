using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CowGrabber : MonoBehaviour
{
    [Header("VR Setup")]
    public Transform vrCamera; // OBLIGATORIO: Arrastra tu Main Camera de XR aquí

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
    }

    void OnTriggerReleased(InputAction.CallbackContext ctx)
    {
        isTriggerHeld = false;

        // Si suelta el gatillo y tenía una vaca, la suelta
        if (grabbedCow != null)
        {
            grabbedCow.transform.SetParent(null); // La vaca queda libre
            grabbedCow.currentCowState = COW_IA.cowStates.Idle; // Vuelve a la normalidad
            grabbedCow = null;
        }

        UpdateLineRenderer(false, Vector3.zero);
    }

    void Update()
    {
        // 1. Lógica para abducir la vaca si ya está agarrada
        if (grabbedCow != null)
        {
            // Vector desde tu cabeza hacia tu mano
            Vector3 dirToHand = (rayOrigin.position - vrCamera.position).normalized;

            // Si el producto punto es negativo, la mano está detrás de la cabeza
            float dot = Vector3.Dot(vrCamera.forward, dirToHand);

            if (dot < -0.2f) // Un pequeño margen para que sea un gesto natural
            {
                TriggerHaptic(1f, 0.5f); // Super vibración al absorberla
                grabbedCow.DestroyCow();
                grabbedCow = null;
                UpdateLineRenderer(false, Vector3.zero);
                return; // Cortamos el Update aquí
            }
        }

        // 2. Lógica del Raycast si está presionando el gatillo pero no tiene vaca
        if (isTriggerHeld && grabbedCow == null)
        {
            Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
            bool hitCow = Physics.Raycast(ray, out RaycastHit hit, rayDistance, cowLayer);

            if (hitCow)
            {
                COW_IA targetCow = hit.collider.GetComponent<COW_IA>();

                if (targetCow != null && targetCow.currentCowState != COW_IA.cowStates.Grabbed)
                {
                    // ¡La atrapamos inmediatamente!
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

        // 3. Actualizar la línea visual hacia la vaca si está agarrada
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
        var device = GetComponent<XRBaseController>();
        if (device != null)
            device.SendHapticImpulse(amplitude, duration);
    }
}