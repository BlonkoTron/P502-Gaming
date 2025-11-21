using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphGenerator : MonoBehaviour
{
    [Header("Graph Containers")]
    public RectTransform graphPanel;
    public RectTransform gridLayer;
    public RectTransform graphLayer;
    public RectTransform labelLayer;
    public RectTransform legendPanel;

    [Header("Prefabs")]
    public GameObject pointPrefab;
    public GameObject linePrefab;
    public GameObject textPrefab;

    [Header("Graph Settings")]
    public float pointSpacing = 100f;
    public int gridLines = 5;

    [Header("Colors")]
    public Color colorA = Color.blue;
    public Color colorB = Color.red;

    [Header("Display Mode")]
    public bool showBents = true;
    // true  = BentsIn + BentsOut
    // false = RotationsUp + RotationsDown

    private List<string> loadedDates;
    private List<DayData> loadedDays;

    /* ---------------- PUBLIC API ---------------- */

    public void InitializeGraph(List<string> dates, List<DayData> days)
    {
        loadedDates = dates;
        loadedDays = days;
        Redraw();
    }

    public void Redraw()
    {
        if (loadedDays == null || loadedDays.Count == 0)
            return;

        ClearGraph();

        // Extract two series depending on the toggle
        List<float> seriesA = new List<float>();
        List<float> seriesB = new List<float>();

        foreach (DayData d in loadedDays)
        {
            if (showBents)
            {
                seriesA.Add(d.nrOfBentsIn);
                seriesB.Add(d.nrOfBentsOut);
            }
            else
            {
                seriesA.Add(d.nrOfRotationsUp);
                seriesB.Add(d.nrOfRotationsDown);
            }
        }

        // Determine max value for scaling
        float maxValue = Mathf.Max(
            Mathf.Max(seriesA.ToArray()),
            Mathf.Max(seriesB.ToArray())
        );

        // Draw grid background
        DrawGrid(maxValue);

        // Plot points & return their UI positions
        var ptsA = PlotLine(seriesA, colorA, true);
        var ptsB = PlotLine(seriesB, colorB, true);

        // Connect lines
        ConnectPoints(ptsA, colorA);
        ConnectPoints(ptsB, colorB);

        // Draw date labels
        DrawDateLabels(loadedDates);

        // Draw legends
        if (showBents)
        {
            DrawLegend("Bents In", colorA);
            DrawLegend("Bents Out", colorB);
        }
        else
        {
            DrawLegend("Rotations Up", colorA);
            DrawLegend("Rotations Down", colorB);
        }
    }

    /* ---------------- CORE GRAPH FUNCTIONS ---------------- */

    private List<Vector2> PlotLine(List<float> values, Color color, bool showLabels)
    {
        List<Vector2> points = new List<Vector2>();
        float max = Mathf.Max(values.ToArray());

        for (int i = 0; i < values.Count; i++)
        {
            float height = (values[i] / max) * graphPanel.sizeDelta.y;
            Vector2 pos = new Vector2(i * pointSpacing, height);
            points.Add(pos);

            // Create point
            GameObject point = Instantiate(pointPrefab, graphLayer);
            point.GetComponent<Image>().color = color;
            point.GetComponent<RectTransform>().anchoredPosition = pos;

            // Value label
            if (showLabels)
            {
                GameObject label = Instantiate(textPrefab, labelLayer);
                label.GetComponent<TMP_Text>().text = values[i].ToString();
                label.GetComponent<RectTransform>().anchoredPosition = pos + new Vector2(0, 25);
            }
        }

        return points;
    }

    private void ConnectPoints(List<Vector2> points, Color color)
    {
        for (int i = 0; i < points.Count - 1; i++)
            DrawLine(points[i], points[i + 1], color);
    }

    private void DrawLine(Vector2 start, Vector2 end, Color color)
    {
        GameObject line = Instantiate(linePrefab, graphLayer);
        Image img = line.GetComponent<Image>();
        img.color = color;

        RectTransform rt = line.GetComponent<RectTransform>();
        Vector2 dir = (end - start).normalized;

        float dist = Vector2.Distance(start, end);
        rt.sizeDelta = new Vector2(dist, 3f);

        rt.anchoredPosition = start + dir * (dist * 0.5f);
        rt.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg);
    }

    /* ---------------- GRID + LABELS ---------------- */

    private void DrawGrid(float maxValue)
    {
        float step = maxValue / gridLines;

        for (int i = 1; i <= gridLines; i++)
        {
            float y = (step * i / maxValue) * graphPanel.sizeDelta.y;

            // Grid line
            GameObject line = Instantiate(linePrefab, gridLayer);
            line.GetComponent<Image>().color = new Color(1, 1, 1, 0.15f);
            RectTransform rt = line.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(graphPanel.sizeDelta.x, 2f);
            rt.anchoredPosition = new Vector2(graphPanel.sizeDelta.x / 2, y);

            // Grid label
            GameObject label = Instantiate(textPrefab, labelLayer);
            label.GetComponent<TMP_Text>().text = (step * i).ToString("0");
            label.GetComponent<RectTransform>().anchoredPosition = new Vector2(-40, y);
        }
    }

    private void DrawDateLabels(List<string> dates)
    {
        for (int i = 0; i < dates.Count; i++)
        {
            GameObject label = Instantiate(textPrefab, labelLayer);
            label.GetComponent<TMP_Text>().text = dates[i];
            label.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * pointSpacing, -40);
        }
    }

    private void DrawLegend(string label, Color color)
    {
        GameObject entry = new GameObject(label, typeof(RectTransform));
        entry.transform.SetParent(legendPanel, false);
        HorizontalLayoutGroup layout = entry.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 10;

        // color square
        GameObject box = new GameObject("Color", typeof(Image));
        box.transform.SetParent(entry.transform, false);
        box.GetComponent<Image>().color = color;
        box.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);

        // label text
        GameObject text = Instantiate(textPrefab, entry.transform);
        text.GetComponent<TMP_Text>().text = label;
    }

    /* ---------------- UTILITY ---------------- */

    private void ClearGraph()
    {
        foreach (Transform t in graphLayer) Destroy(t.gameObject);
        foreach (Transform t in gridLayer) Destroy(t.gameObject);
        foreach (Transform t in labelLayer) Destroy(t.gameObject);
        foreach (Transform t in legendPanel) Destroy(t.gameObject);
    }
}