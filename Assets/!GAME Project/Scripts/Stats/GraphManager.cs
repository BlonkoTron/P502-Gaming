using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public GraphGenerator graphGenerator;
    public DailySaveSystem saveSystem;

    void Start()
    {
        DaysSave save = saveSystem.Load();

        graphGenerator.InitializeGraph(save.dates, save.data);
    }
}
