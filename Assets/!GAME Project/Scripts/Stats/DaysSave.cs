using System.Collections.Generic;

[System.Serializable]
public class DaysSave
{
    public List<string> dates = new List<string>();
    public List<DayData> data = new List<DayData>();

    public void Set(string date, DayData dayData)
    {
        int index = dates.IndexOf(date);

        if (index >= 0)
            data[index] = dayData;           // overwrite today’s data
        else
        {
            dates.Add(date);
            data.Add(dayData);               // add new day
        }
    }

    public DayData Get(string date)
    {
        int index = dates.IndexOf(date);
        if (index >= 0) return data[index];
        return null;
    }
}