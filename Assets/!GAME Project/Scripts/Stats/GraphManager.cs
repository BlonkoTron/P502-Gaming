using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public GraphGenerator graph;
    public DailySaveSystem saveSystem;

    void Start()
    {
        DaysSave save = saveSystem.Load();

        graph.InitializeGraph(save.dates, save.data);
    }
}