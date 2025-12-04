using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GraphGenerator : MonoBehaviour
{
    public RectTransform graphPanel;  // Parent panel (280×160)
    public GameObject pointPrefab;
    public GameObject linePrefab;
    public GameObject labelPrefab;

    public bool showBents = true;   // Switch between stats

    private float graphWidth;
    private float graphHeight;

    // Internal storage
    private List<string> dates;
    private List<DayData> days;



    // Public API - call this to provide data then draw
    public void InitializeGraph(List<string> datesList, List<DayData> daysList)
    {
        this.dates = datesList ?? new List<string>();
        this.days = daysList ?? new List<DayData>();
        Redraw();
    }


    void Start()
    {
        Redraw();
    }

    public void Redraw()
    {
        if (graphPanel == null || days == null || dates == null)
            return;

        // Clear old graph objects
        foreach (Transform child in graphPanel)
            Destroy(child.gameObject);

        graphWidth = graphPanel.rect.width;
        graphHeight = graphPanel.rect.height;

        int count = days.Count;
        if (count == 0) return;

        float xStep = graphWidth / (count + 1);

        // Extract values depending on toggle
        List<float> valsA = new List<float>();
        List<float> valsB = new List<float>();

        for (int i = 0; i < count; i++)
        {
            if (showBents)
            {
                valsA.Add(days[i].nrOfBentsIn);
                valsB.Add(days[i].nrOfBentsOut);
            }
            else
            {
                valsA.Add(days[i].nrOfRotationsUp);
                valsB.Add(days[i].nrOfRotationsDown);
            }
        }

        float maxValue = Mathf.Max(Max(valsA), Max(valsB));
        float yScale = graphHeight / (maxValue * 1.2f);

        float dotSize = graphWidth * 0.03f;
        float labelSize = graphWidth * 0.045f;
        float lineThickness = graphWidth * 0.01f;

        List<Vector2> positionsA = new List<Vector2>();
        List<Vector2> positionsB = new List<Vector2>();

        for (int i = 0; i < count; i++)
        {
            float x = (i + 1) * xStep;

            float yA = valsA[i] * yScale;
            float yB = valsB[i] * yScale;

            Vector2 posA = new Vector2(x, yA);
            Vector2 posB = new Vector2(x, yB);

            positionsA.Add(posA);
            positionsB.Add(posB);

            // Draw A
            CreatePoint(posA, Color.red, dotSize);
            CreateLabel(posA + Vector2.up * (labelSize * 0.5f), valsA[i].ToString(), labelSize);

            // Draw B
            CreatePoint(posB, Color.blue, dotSize);
            CreateLabel(posB + Vector2.up * (labelSize * 0.5f), valsB[i].ToString(), labelSize);

            // Date labels
            CreateLabel(new Vector2(x, -labelSize * 1.2f), dates[i], labelSize * 0.8f);
        }

        DrawLines(positionsA, Color.red, lineThickness);
        DrawLines(positionsB, Color.blue, lineThickness);
    }

    void CreatePoint(Vector2 position, Color color, float size)
    {
        GameObject p = Instantiate(pointPrefab, graphPanel);
        RectTransform rt = p.GetComponent<RectTransform>();

        rt.sizeDelta = new Vector2(size, size);
        rt.anchoredPosition = position;

        Image img = p.GetComponent<Image>();
        img.color = color;
    }

    void CreateLabel(Vector2 position, string text, float fontSize)
    {
        GameObject label = Instantiate(labelPrefab, graphPanel);
        RectTransform rt = label.GetComponent<RectTransform>();

        rt.anchoredPosition = position;

        TMP_Text txt = label.GetComponent<TMP_Text>();
        txt.text = text;
        txt.fontSize = fontSize;
    }

    void DrawLines(List<Vector2> points, Color color, float thickness)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 a = points[i];
            Vector2 b = points[i + 1];

            GameObject line = Instantiate(linePrefab, graphPanel);
            RectTransform rt = line.GetComponent<RectTransform>();

            Vector2 diff = b - a;
            float length = diff.magnitude;

            rt.sizeDelta = new Vector2(length, thickness);
            rt.anchoredPosition = a + diff / 2f;
            rt.localRotation = Quaternion.FromToRotation(Vector3.right, diff);

            Image img = line.GetComponent<Image>();
            img.color = color;
        }
    }

    float Max(List<float> list)
    {
        float max = float.MinValue;
        foreach (var f in list)
            if (f > max) max = f;
        return max;
    }
}
