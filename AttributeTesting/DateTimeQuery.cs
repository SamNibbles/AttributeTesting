namespace AttributeTesting;

public class DateTimeQuery : BaseQuery
{
    [Query("startDate", typeof(QueryDateTimeSerializer))]
    public DateTime StartDate { get; set; }

    [Query("endDate", typeof(QueryDateTimeSerializer))]
    public DateTime EndDate { get; set; }

    [Query("timePeriod", typeof(QueryTimePeriodSerializer))]
    public TimePeriod TimePeriod { get; set; }
}

public static class QueryDateTimeSerializer
{
    public static DateTime SerializeQueryParameter(string dateTime)
    {
        return DateTime.Parse(dateTime);
    }

    public static string DeserializeQueryParameter(DateTime dateTime)
    {
        return dateTime.ToString("yyyyMMdd");
    }
}

public static class QueryTimePeriodSerializer
{
    public static TimePeriod SerializeQueryParameter(string timePeriod)
    {
        return Enum.Parse<TimePeriod>(timePeriod);
    }    
    
    public static string DeserializeQueryParameter(TimePeriod timePeriod)
    {
        return timePeriod.ToString();
    }
}