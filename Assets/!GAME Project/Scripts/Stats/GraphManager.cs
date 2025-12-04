using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public GraphGenerator graph;     // <-- reference to the new script
    public DailySaveSystem saveSystem;

    void Start()
    {
        DaysSave save = saveSystem.Load();

        graph.InitializeGraph(save.dates, save.data);
    }
}