using System.Collections.Generic;
using UnityEngine;


public class COW_IA : MonoBehaviour
{
    public GameObject prefabForPoints;

    public List<Transform> collisionPoints = new List<Transform>();
    
    

    public float radius;
    

    public enum cowStates
    {
        Idle,
        Eat,
        Moving,
        Run
    }

    public cowStates currentCowState;


    void Start()
    {
        initialitePoints();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChoosePoint();
        }
    }

    public void GenerateCircle()
    {

        collisionPoints.Clear();
        Collider[] hitColliders =  Physics.OverlapSphere(transform.position, radius);



        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            Debug.Log(distance);
                collisionPoints.Add(collider.transform);
                Debug.Log(collider.transform.name);     

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
        if (collisionPoints.Count > 0)
        {
            foreach (Transform pos in collisionPoints)
            {
                Gizmos.DrawSphere(pos.transform.position, 1.1f);
            }
        }

       
    }

    private void ChangeStateCow()
    {

    }

    private void initialitePoints()
    {
        Vector3 currentPos = transform.position;    
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45;
            Quaternion rot = Quaternion.Euler(0, angle, 0);
            Vector3 spawnOffset = new Vector3(
                   Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                   0,
                   Mathf.Cos(angle * Mathf.Deg2Rad) * radius
               );
            Vector3 spawnPos = currentPos + spawnOffset;
            GameObject newPoint = Instantiate(prefabForPoints,spawnPos,rot);
            collisionPoints.Add(newPoint.transform);
        }

    }
    private void ChoosePoint()
    {
        int choose = Random.Range(0, collisionPoints.Count - 1);
        Transform choose2 = collisionPoints[choose].transform;

        MoveCowToPoint(choose2);


    }

    private void MoveCowToPoint(Transform destinty)
    {
        Vector3 currentPos = transform.position;
        currentPos.x =  Mathf.MoveTowards(transform.position.x, destinty.transform.position.x,2.0f);
        currentPos.z =  Mathf.MoveTowards(transform.position.z, destinty.transform.position.z,2.0f);
        transform.position = currentPos;
    }
}
