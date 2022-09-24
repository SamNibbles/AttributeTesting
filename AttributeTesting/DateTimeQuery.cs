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
    public static DateTime DeserializeQueryParameter(string dateTime)
    {
        return DateTime.Parse(dateTime);
    }

    public static string SerializeQueryParameter(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }
}

public static class QueryTimePeriodSerializer
{
    public static TimePeriod DeserializeQueryParameter(string timePeriod)
    {
        return Enum.Parse<TimePeriod>(timePeriod);
    }    
    
    public static string SerializeQueryParameter(TimePeriod timePeriod)
    {
        return timePeriod.ToString();
    }
}