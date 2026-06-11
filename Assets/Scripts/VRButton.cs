using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class VRButton : MonoBehaviour
{
    [Header("Eventos del Botón")]
    [Tooltip("Arrastra aqui lo que quieres que pase al hacer click")]
    public UnityEvent onButtonClick;

    [Header("Feedback Visual")]
    public float distanciaHundimiento = 0.05f;
    public float duracionAnimacion = 0.15f;

    private Vector3 posicionOriginal;
    private bool estaPresionado = false;

    void Start()
    {
        posicionOriginal = transform.localPosition;
    }

    public void Click()
    {
        if (estaPresionado) return;

        estaPresionado = true;

        transform.DOLocalMoveZ(posicionOriginal.z + distanciaHundimiento, duracionAnimacion)
                 .SetLoops(2, LoopType.Yoyo)
                 .OnComplete(() => estaPresionado = false);

        onButtonClick.Invoke();
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
}