[System.Serializable]
public class DayData
{
    public int nrOfBentsIn;
    public int nrOfBentsOut;
    public int nrOfRotationsUp;
    public int nrOfRotationsDown;

    public DayData(PlayerStats stats)
    {
        nrOfBentsIn = stats.nrOfBentsIn;
        nrOfBentsOut = stats.nrOfBentsOut;
        nrOfRotationsUp = stats.nrOfRotationsUp;
        nrOfRotationsDown = stats.nrOfRotationsDown;
    }
}
