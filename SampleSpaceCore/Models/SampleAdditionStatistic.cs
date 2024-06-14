namespace SampleSpaceCore.Models;

public class SampleAdditionStatistic
{
    private SampleAdditionStatistic(DateOnly date, int count)
    {
        Date = date;
        Count = count;

        if (count > 0)
            Level = 1;

        if (count > 2)
            Level = 2;
    }
    
    public DateOnly Date { get; private set; }

    public int Count { get; private set; }

    public int Level { get; private set; } = 0;
    
    public void AddCount(int count)
    {
        Count += count;
        
        if (count > 0)
            Level = 1;

        if (count > 2)
            Level = 2;
    }

    public static SampleAdditionStatistic Create(DateOnly date, int count)
    {
        var sampleAdditionStatistics = new SampleAdditionStatistic(date, count);

        return sampleAdditionStatistics;
    }
}