using UnityEngine;
using DG.Tweening;

public class ArrowLogic : MonoBehaviour
{
    private Transform playerT;

    public float distanciaRebote = 1.0f;
    public float duracionRebote = 0.6f;

    public bool corregirRotacion = true;
    public Vector3 ajusteRotacion = new Vector3(90f, 0f, 0f);

    void Start()
    {
        playerT = FindFirstObjectByType<CharacterController>().transform;

        transform.DOMoveY(transform.position.y + distanciaRebote, duracionRebote)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
    }

    void Update()
    {
        if (playerT != null)
        {
            transform.LookAt(playerT);
            if (corregirRotacion)
            {
                transform.Rotate(ajusteRotacion);
            }
        }
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
}