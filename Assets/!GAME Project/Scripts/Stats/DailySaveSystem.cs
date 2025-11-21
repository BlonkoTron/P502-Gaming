using UnityEngine;

public class DailySaveSystem : MonoBehaviour
{
    public PlayerStats playerStats;

    private const string LAST_SAVE_DATE_KEY = "last_save_date";

    private void Start()
    {
        CheckForDailyReset();
    }

    private void CheckForDailyReset()
    {
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");

        string lastSaved = PlayerPrefs.GetString(LAST_SAVE_DATE_KEY, "");

        if (lastSaved != today)
        {
            ResetPlayerStats();
            PlayerPrefs.SetString(LAST_SAVE_DATE_KEY, today);
            PlayerPrefs.Save();
        }
    }

    private void ResetPlayerStats()
    {
        playerStats.nrOfBentsIn = 0;
        playerStats.nrOfBentsOut = 0;
        playerStats.nrOfRotationsUp = 0;
        playerStats.nrOfRotationsDown = 0;
    }

    public void SaveToday()
    {
        DaysSave save = Load();

        string today = System.DateTime.Now.ToString("yyyy-MM-dd");

        DayData newDayData = new DayData(playerStats);
        save.Set(today, newDayData);

        string json = JsonUtility.ToJson(save, true);
        PlayerPrefs.SetString("daily_stats", json);
        PlayerPrefs.Save();

        Debug.Log("Saved stats for " + today);
    }

    public DaysSave Load()
    {
        if (!PlayerPrefs.HasKey("daily_stats"))
            return new DaysSave();

        return JsonUtility.FromJson<DaysSave>(PlayerPrefs.GetString("daily_stats"));
    }
}