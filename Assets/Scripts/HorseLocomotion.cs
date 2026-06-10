using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HorseLocomotion : MonoBehaviour
{
    public Animator anim;



    private void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    public void Run()
    {
        if (anim != null)
        {
            anim.SetFloat("Vert", 1.0f);
            anim.SetFloat("State", 1.0f);
        }
    }


}