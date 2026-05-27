using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class COW_IA : MonoBehaviour
{
    private List<Vector3> collisionPoints = new List<Vector3>();
    
    public float radius;
    

    public enum cowStates
    {
        Idle,
        Eat,
        Moving,
        Run
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateCircle()
    {
        Collider[] hitColliders =  Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in hitColliders)
        {
            collisionPoints.Add(collider.transform.position);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach(Vector3 pos in collisionPoints)
        {
            Gizmos.DrawSphere(pos, radius);
        }
    }
}
