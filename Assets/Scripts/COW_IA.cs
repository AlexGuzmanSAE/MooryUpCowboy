using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;


public class COW_IA : MonoBehaviour
{
    public GameObject prefabForPoints;

    public List<GameObject> collisionPoints = new List<GameObject>();



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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChoosePoint();
        }
        if (currentCowState == cowStates.Idle)
        {
            StartCoroutine(waitToChoosePoint());
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
        if (collisionPoints.Count > 0)
        {
            foreach (GameObject pos in collisionPoints)
            {
                Gizmos.DrawSphere(pos.transform.position, 1.1f);
            }
        }


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
            GameObject newPoint = Instantiate(prefabForPoints, spawnPos, rot);
            collisionPoints.Add(newPoint);
        }
       // StartCoroutine(waitToChoosePoint());

    }

    public void MovePointsCow()
    {
        if (collisionPoints.Count > 0)
        {
            Vector3 currentPos = transform.position;
            float i = 0;
            foreach (GameObject pos in collisionPoints)
            {
                float angle = i * 45;
                Quaternion rot = Quaternion.Euler(0, angle, 0);
                Vector3 spawnOffset = new Vector3(
                       Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                       0,
                       Mathf.Cos(angle * Mathf.Deg2Rad) * radius
                   );
                i++;
                Vector3 spawnPos = currentPos + spawnOffset;
                Debug.Log(i);
                pos.transform.position = spawnPos;
            }
        }
    }

    private void ChoosePoint()
    {
        if (collisionPoints.Count > 0)
        {
            int choose = Random.Range(0, collisionPoints.Count);
            Transform choose2 = collisionPoints[choose].transform;

            MoveCowToPoint(choose2);
        }

    }

    private void MoveCowToPoint(Transform destinty)
    {
        currentCowState = cowStates.Moving;
        Vector3 currentPos = transform.position;
        this.transform.LookAt(destinty.transform);
        this.gameObject.transform.DOMove(destinty.position, 1.0f).OnComplete(() =>
        {
            MovePointsCow();
            currentCowState = cowStates.Idle;

        } );

    }

    IEnumerator waitToChoosePoint()
    {
       
            currentCowState = cowStates.Moving;
            yield return new WaitForSeconds(2);
            ChoosePoint();
            Debug.Log("Corruitna");
        
     
    }
}
