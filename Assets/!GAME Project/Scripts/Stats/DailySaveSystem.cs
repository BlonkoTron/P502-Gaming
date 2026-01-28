using UnityEngine;

public class DailySaveSystem : MonoBehaviour
{
    public PlayerStats playerStats;

    private const string LAST_SAVE_DATE_KEY = "last_save_date";

    private void Start()
    {
        // Check if a new day has started and reset stats if needed
        CheckForDailyReset();
    }

    // Compares today's date with the last saved date
    private void CheckForDailyReset()
    {
        // Get today's date in a consistent format (YYYY-MM-DD)
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");

        // Load the last saved date (empty string if none exists)
        string lastSaved = PlayerPrefs.GetString(LAST_SAVE_DATE_KEY, "");

        // If the date has changed, reset stats and update the saved date
        if (lastSaved != today)
        {
            ResetPlayerStats();
            PlayerPrefs.SetString(LAST_SAVE_DATE_KEY, today);
            PlayerPrefs.Save();
        }
    }

    // Resets all daily player statistics back to zero
    private void ResetPlayerStats()
    {
        playerStats.nrOfBentsIn = 0;
        playerStats.nrOfBentsOut = 0;
        playerStats.nrOfRotationsUp = 0;
        playerStats.nrOfRotationsDown = 0;
    }

    // Saves the current player's stats for today
    public void SaveToday()
    {
        // Load existing saved data (or create a new one)
        DaysSave save = Load();

        // Get today's date as the key
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");

        // Create a new DayData object from the current player stats
        DayData newDayData = new DayData(playerStats);
        // Store today's data in the save structure
        save.Set(today, newDayData);

        // Convert the save data to JSON and store it in PlayerPrefs
        string json = JsonUtility.ToJson(save, true);
        PlayerPrefs.SetString("daily_stats", json);
        PlayerPrefs.Save();

        // Debug message to confirm the save
        Debug.Log("Saved stats for " + today);
    }

    // Loads all saved daily stats from PlayerPrefs
    public DaysSave Load()
    {
        // If no save exists yet, return a new empty save object
        if (!PlayerPrefs.HasKey("daily_stats"))
            return new DaysSave();

        // Deserialize the JSON data back into a DaysSave object
        return JsonUtility.FromJson<DaysSave>(PlayerPrefs.GetString("daily_stats"));
    }
}