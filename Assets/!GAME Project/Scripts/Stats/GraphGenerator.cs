using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphGenerator : MonoBehaviour
{
    public RectTransform graphPanel;
    public GameObject pointPrefab;
    public Color bentsInColor = Color.blue;
    public Color bentsOutColor = Color.red;
    public float pointSpacing = 80f;

    public void DrawGraph(List<DayData> days)
    {
        // Clear old graph
        foreach (Transform child in graphPanel)
            Destroy(child.gameObject);

        if (days.Count == 0)
            return;

        // Extract both values
        List<float> bentsIn = new List<float>();
        List<float> bentsOut = new List<float>();

        foreach (DayData d in days)
        {
            bentsIn.Add(d.nrOfBentsIn);
            bentsOut.Add(d.nrOfBentsOut);
        }

        // Find max for scaling
        float maxValue = Mathf.Max(
            Mathf.Max(bentsIn.ToArray()),
            Mathf.Max(bentsOut.ToArray())
        );

        // Create lists to store point UI positions
        List<Vector2> inPoints = new List<Vector2>();
        List<Vector2> outPoints = new List<Vector2>();

        // Create UI points for BOTH lines
        for (int i = 0; i < days.Count; i++)
        {
            float inHeight = (bentsIn[i] / maxValue) * graphPanel.sizeDelta.y;
            float outHeight = (bentsOut[i] / maxValue) * graphPanel.sizeDelta.y;

            Vector2 inPos = new Vector2(i * pointSpacing, inHeight);
            Vector2 outPos = new Vector2(i * pointSpacing, outHeight);

            inPoints.Add(inPos);
            outPoints.Add(outPos);

            // Create points
            CreatePoint(inPos, bentsInColor);
            CreatePoint(outPos, bentsOutColor);
        }

        // Connect points with lines
        CreateLineSegments(inPoints, bentsInColor);
        CreateLineSegments(outPoints, bentsOutColor);
    }

    private void CreatePoint(Vector2 position, Color color)
    {
        GameObject point = Instantiate(pointPrefab, graphPanel);
        Image img = point.GetComponent<Image>();
        img.color = color;

        RectTransform rt = point.GetComponent<RectTransform>();
        rt.anchoredPosition = position;
    }

    private void CreateLineSegments(List<Vector2> points, Color color)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            CreateLine(points[i], points[i + 1], color);
        }
    }

    private void CreateLine(Vector2 start, Vector2 end, Color color)
    {
        GameObject lineObj = new GameObject("Line", typeof(Image));
        lineObj.transform.SetParent(graphPanel, false);

        Image img = lineObj.GetComponent<Image>();
        img.color = color;

        RectTransform rt = lineObj.GetComponent<RectTransform>();

        Vector2 direction = (end - start).normalized;
        float length = Vector2.Distance(start, end);

        rt.sizeDelta = new Vector2(length, 3f); // line thickness
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
        rt.anchoredPosition = start + direction * length * 0.5f;
        rt.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg);
    }
}