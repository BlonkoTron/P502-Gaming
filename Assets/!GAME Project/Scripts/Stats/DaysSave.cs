using System.Collections.Generic;

[System.Serializable]
public class DaysSave
{
    // List of dates (formatted as YYYY-MM-DD)
    // Each index corresponds to the same index in the data list
    public List<string> dates = new List<string>();
    // List of saved data for each date
    public List<DayData> data = new List<DayData>();

    // Adds or updates the saved data for a specific date
    public void Set(string date, DayData dayData)
    {
        // Check if the date already exists in the list
        int index = dates.IndexOf(date);

        if (index >= 0)
        { // If the date exists, overwrite the existing data}
            data[index] = dayData;
        }
        else
        {
            // If the date does not exist, add a new entry
            dates.Add(date);
            data.Add(dayData);
        }
    }

    // Retrieves saved data for a specific date
    public DayData Get(string date)
    {
        // Find the index of the requested date
        int index = dates.IndexOf(date);
        // Return the data if found, otherwise return null
        if (index >= 0) return data[index];
        return null;
    }
}