using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private InputActionReference leftHand;
    public Vector3 velocity;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           
        if(leftHand == null)
        {
            Debug.Log("NO HAND");
        }
    }

    // Update is called once per frame
    void Update()
    {
       if( leftHand != null)
        {
            velocity = leftHand.action.ReadValue<Vector3>();
            
        }
        else
        {
            Debug.Log("NO");
        }

       
    }
}
