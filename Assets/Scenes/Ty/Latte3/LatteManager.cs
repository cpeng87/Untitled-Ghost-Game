using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LatteManager : MonoBehaviour
{
    [SerializeField] LatteDraw traceDrawer;
    [SerializeField] DisplayShape targetShape;
    [SerializeField] TMP_Text accuracyText;
    [SerializeField] int targetSampleCount = 100;
    [SerializeField] float coverageTolerance = 0.15f;

    void Update()
    {
        Evaluate();
    }

    public void Evaluate()
    {
        List<Vector3> tracedPoints = traceDrawer.GetTracedPoints();
        List<Vector3> targetPointsRaw = targetShape.GetShapePointsWorld();

        List<Vector3> targetSamples = ResamplePath(targetPointsRaw, targetSampleCount);
        if (targetSamples.Count == 0)
        {
            SetAccuracyText(0f);
            return;
        }

        int covered = 0;
        foreach (Vector3 targetPoint in targetSamples)
        {
            float nearestDist = float.MaxValue;
            foreach (Vector3 tracedPoint in tracedPoints)
            {
                float d = Vector3.Distance(targetPoint, tracedPoint);
                if (d < nearestDist) nearestDist = d;
            }

            if (nearestDist <= coverageTolerance)
                covered++;
        }

        float accuracy = (covered / (float)targetSamples.Count) * 100f;
        SetAccuracyText(accuracy);
    }

    void SetAccuracyText(float accuracy)
    {
        if (accuracyText != null)
        {
            accuracyText.text = $"{accuracy:0}%";
        }
    }

    static List<Vector3> ResamplePath(List<Vector3> points, int sampleCount)
    {
        if (points == null || points.Count < 2 || sampleCount < 2) return new List<Vector3>();

        //idk
        int wrapCount = points.Count + 1; 
        var cumulative = new float[wrapCount];
        cumulative[0] = 0f;
        for (int i = 1; i < wrapCount; i++)
        {
            Vector3 a = points[(i - 1) % points.Count];
            Vector3 b = points[i % points.Count];
            cumulative[i] = cumulative[i - 1] + Vector3.Distance(a, b);
        }

        float totalLength = cumulative[wrapCount - 1];
        if (totalLength <= Mathf.Epsilon) return new List<Vector3>();

        int segIndex = 0;
        List<Vector3> result = new List<Vector3>();
        for (int s = 0; s < sampleCount; s++)
        {
            float targetDist = totalLength * (s / (float)(sampleCount - 1));

            while (segIndex < wrapCount - 2 && cumulative[segIndex + 1] < targetDist)
                segIndex++;

            float segStart = cumulative[segIndex];
            float segEnd = cumulative[segIndex + 1];
            float segLength = segEnd - segStart;
            float t = segLength > Mathf.Epsilon ? (targetDist - segStart) / segLength : 0f;

            Vector3 a = points[segIndex % points.Count];
            Vector3 b = points[(segIndex + 1) % points.Count];

            result.Add(Vector3.Lerp(a, b, t));
        }

        return result;
    }
}