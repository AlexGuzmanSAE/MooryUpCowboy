using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;


public class COW_IA : MonoBehaviour
{
    public GameObject prefabForPoints;

    public List<GameObject> collisionPoints = new List<GameObject>();

    public Animator animator;



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
        if(animator == null)
        {
            animator = GetComponent<Animator>();    
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCowState == cowStates.Idle)
        {
            StartCoroutine(waitToChooseAction());
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
        animator.SetBool("IsWalking", true);
        Vector3 currentPos = transform.position;
        this.transform.LookAt(destinty.transform);
        this.gameObject.transform.DOMove(destinty.position, 2.5f).OnComplete(() =>
        {
            MovePointsCow();
            currentCowState = cowStates.Idle;
            animator.SetBool("IsWalking", false);

        } );

    }

    IEnumerator waitToChooseAction()
    {
        int random = Random.Range(0, 11);
        if(random < 7)
        {
            currentCowState = cowStates.Eat;
            animator.SetBool("IsEating", true);
            yield return new WaitForSeconds(Random.Range(4, 7));
            animator.SetBool("IsEating", false);
            currentCowState = cowStates.Idle;

        }
        else
        {
            currentCowState = cowStates.Moving;
            yield return new WaitForSeconds(Random.Range(4, 10));
            ChoosePoint();
            Debug.Log("Corruitna");
        }
          
        
     
    }
}
