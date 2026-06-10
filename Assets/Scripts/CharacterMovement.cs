using UnityEngine;

public class ArmSwingLocomotion : MonoBehaviour
{
    [Header("Referencias de las Manos")]

    public Transform manoIzquierda;
    public Transform manoDerecha;

    public Transform direccionCabeza;

    [Header("Ajustes de Movimiento")]
    public float multiplicadorVelocidad = 5.0f;
    public float umbralMinimo = 0.01f; 

    private Vector3 posAnteriorIzquierda;
    private Vector3 posAnteriorDerecha;

    void Start()
    {
        if (manoIzquierda != null) posAnteriorIzquierda = manoIzquierda.localPosition;
        if (manoDerecha != null) posAnteriorDerecha = manoDerecha.localPosition;
    }

    void Update()
    {

        Vector3 posActualIzquierda = manoIzquierda.localPosition;
        Vector3 posActualDerecha = manoDerecha.localPosition;


        Vector3 deltaIzquierda = posActualIzquierda - posAnteriorIzquierda;
        Vector3 deltaDerecha = posActualDerecha - posAnteriorDerecha;


        float movimientoBrazos = Mathf.Abs(deltaIzquierda.y) + Mathf.Abs(deltaDerecha.y);


        if (movimientoBrazos > umbralMinimo)
        {

            Vector3 direccionMovimiento = direccionCabeza.forward;
            direccionMovimiento.y = 0; 

     
            transform.position += direccionMovimiento.normalized * movimientoBrazos * multiplicadorVelocidad * Time.deltaTime;
        }

        
        posAnteriorIzquierda = posActualIzquierda;
        posAnteriorDerecha = posActualDerecha;
    }
}