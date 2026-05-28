using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.DebugUI.Table;

public class Cow_Collector : MonoBehaviour
{
    [SerializeField] private int cowsCollected = 0;
    public int requiredPoints = 20;
    [SerializeField] private TextMeshProUGUI scoreTxt;

    public Transform hand;

    public float recordTime = 1f;

    public float circleRadiusTolerance = 0.05f;

    public float captureRange = 5f;

    private List<Vector3> positions = new List<Vector3>();
    private float timer;

    public void AddScore(int amount)
    {
        cowsCollected += amount;
        scoreTxt.text = "Score: " + scoreTxt;
    }

    void Update()
    {
        RecordHandPosition();

        if (DetectCircle())
        {
            CaptureCow();
            positions.Clear();
        }
    }

    void RecordHandPosition()
    {
        timer += Time.deltaTime;

        positions.Add(hand.position);

        if (positions.Count > requiredPoints)
        {
            positions.RemoveAt(0);
        }
    }

    bool DetectCircle()
    {
        if (positions.Count < requiredPoints)
            return false;

        Vector3 center = Vector3.zero;

        foreach (var pos in positions)
        {
            center += pos;
        }

        center /= positions.Count;
        float avgRadius = 0f;

        foreach (var pos in positions)
        {
            avgRadius += Vector3.Distance(center, pos);
        }

        avgRadius /= positions.Count;

        float variance = 0f;

        foreach (var pos in positions)
        {
            float dist = Vector3.Distance(center, pos);
            variance += Mathf.Abs(dist - avgRadius);
        }

        variance /= positions.Count;

        return variance < circleRadiusTolerance;
    }

    void CaptureCow()
    {
        Collider[] hits = Physics.OverlapSphere(hand.position, captureRange);

        foreach (Collider hit in hits)
        {
            Cow_Info cow = hit.GetComponent<Cow_Info>();

            if (cow != null)
            {
                AddScore(cow.points);
                Destroy(cow.gameObject);

                Debug.Log("Vaca capturada!");
                break;
            }
        }
    }

}
