using Unity.XR.CoreUtils;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]
public class HorseLocomotion : MonoBehaviour
{
    
    public XROrigin headset;





    private void Start()
    {

    }

    private void Update()
    {
        
    }

    private void RotateHorse()
    {
        if (headset != null)
        {
            Vector3 actualRot = this.transform.rotation.eulerAngles;
        }
    }


}