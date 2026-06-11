using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowSpawner : MonoBehaviour
{
    static public CowSpawner instance;
    public GameObject CowPrefab;

    public List<Transform> cowSpawnPoints;
    bool isSpawning;
    public Transform CowParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cowSpawnPoints = new List<Transform>();
        FillList();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning)
        {
            StartCoroutine(AwaitToSpawnCow());
        }
    }

    void FillList()
    {
        cowSpawnPoints.Clear();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            cowSpawnPoints.Add(transform.GetChild(i));
            
        }
    }

    public void SpawnCow()
    {
        if (CowPrefab != null)
        {
            int choose =  Random.Range(0, cowSpawnPoints.Count);
        
            Instantiate(CowPrefab, cowSpawnPoints[choose].position, Quaternion.identity,CowParent);
        }
        else
        {
            Debug.LogWarning("No se asigno el prefab de la vaca");
        }
      
    }

    IEnumerator AwaitToSpawnCow()
    {
        isSpawning = true;  
       yield return new WaitForSeconds(Random.Range(1f, 5f));
       SpawnCow();
       isSpawning = false;
       
    }
}