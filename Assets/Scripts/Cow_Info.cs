using UnityEngine;

public class Cow_Info : MonoBehaviour
{
    public int points = 1;

    private bool isGrabbed;
    private Transform grabber;
    private Vector3 offset;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GetGrabbed(Transform grabTransform)
    {
        if (isGrabbed) return;

        isGrabbed = true;
        grabber = grabTransform;
        offset = transform.position - grabber.position;

        if (rb) rb.isKinematic = true;

        Debug.Log($"{gameObject.name} fue agarrada!");
        // Animación, sonido
    }

    public void Release()
    {
        isGrabbed = false;
        if (rb) rb.isKinematic = false;
        grabber = null;
    }

    void Update()
    {
        if (isGrabbed && grabber != null)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                grabber.position + offset,
                Time.deltaTime * 10f
            );
        }
    }
}
