using DG.Tweening;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Referencias de las Manos")]
    public Transform manoIzquierda;
    public Transform manoDerecha;
    public Transform direccionCabeza;

    [Header("Ajustes de Movimiento")]
    public float multiplicadorVelocidad = 5.0f;
    public float umbralMinimo = 0.01f;
    public float velocidadRotacionCaballo = 5.0f;

    [Header("Ajustes de Inercia")]
    public float aceleracion = 5.0f; // Qué tan rápido alcanza la velocidad al mover las manos
    public float desaceleracion = 2.0f; // Qué tan suavemente frena al dejar de moverlas

    [Header("Caballito")]
    public Transform caballoT;

    private Vector3 posAnteriorIzquierda;
    private Vector3 posAnteriorDerecha;

    // Nueva variable para guardar el impulso actual
    private float velocidadActual = 0f;

    public HorseLocomotion horseLocomotion;

    void Start()
    {
        if (manoIzquierda != null) posAnteriorIzquierda = manoIzquierda.localPosition;
        if (manoDerecha != null) posAnteriorDerecha = manoDerecha.localPosition;
    }

    void Update()
    {
        if (manoIzquierda == null || manoDerecha == null || direccionCabeza == null) return;

        Vector3 posActualIzquierda = manoIzquierda.localPosition;
        Vector3 posActualDerecha = manoDerecha.localPosition;
        Vector3 deltaIzquierda = posActualIzquierda - posAnteriorIzquierda;
        Vector3 deltaDerecha = posActualDerecha - posAnteriorDerecha;

        float anguloObjetivo = GetTargetAngle();
        HandleHorseRotation(anguloObjetivo);

        float movimientoBrazos = Mathf.Abs(deltaIzquierda.y) + Mathf.Abs(deltaDerecha.y);

        float velocidadObjetivo = 0f;
        if (movimientoBrazos > umbralMinimo)
        {
            velocidadObjetivo = movimientoBrazos * multiplicadorVelocidad;
            horseLocomotion.Run();
        }

        float factorCambio = (velocidadObjetivo > velocidadActual) ? aceleracion : desaceleracion;
        velocidadActual = Mathf.Lerp(velocidadActual, velocidadObjetivo, factorCambio * Time.deltaTime);

        if (velocidadActual > 0.001f)
        {
            Vector3 direccionMovimiento = direccionCabeza.forward;
            direccionMovimiento.y = 0;


            transform.position += direccionMovimiento.normalized * velocidadActual * Time.deltaTime;
        }

        posAnteriorIzquierda = posActualIzquierda;
        posAnteriorDerecha = posActualDerecha;
    }

    float GetTargetAngle()
    {
        Vector3 forward = direccionCabeza.forward;
        forward.y = 0;
        return Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
    }

    void HandleHorseRotation(float targetYAngle)
    {
        if (caballoT == null) return;

        Quaternion rotacionObjetivo = Quaternion.Euler(0, targetYAngle, 0);
        caballoT.rotation = Quaternion.Slerp(caballoT.rotation, rotacionObjetivo, velocidadRotacionCaballo * Time.deltaTime);
    }
}