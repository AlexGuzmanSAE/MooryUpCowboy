using UnityEngine;
using UnityEngine.InputSystem;

public class VRButtonPointer : MonoBehaviour
{
    [Header("Raycast Setup")]
    public Transform rayOrigin;
    public float maxDistance = 5f;
    public LayerMask buttonLayer;  

    [Header("Input Setup")]
    public InputActionReference triggerAction;

    void OnEnable()
    {
        triggerAction.action.started += OnTriggerPressed;
        triggerAction.action.Enable();
    }

    void OnDisable()
    {
        triggerAction.action.started -= OnTriggerPressed;
        triggerAction.action.Disable();
    }

    void OnTriggerPressed(InputAction.CallbackContext ctx)
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, buttonLayer))
        {
            VRButton botonDetectado = hit.collider.GetComponent<VRButton>();

            if (botonDetectado != null)
            {
                botonDetectado.Click();
            }
        }
    }
}