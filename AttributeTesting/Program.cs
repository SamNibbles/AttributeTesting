using AttributeTesting;

var dateTimeQuery = new DateTimeQuery
{
    StartDate = DateTime.UtcNow,
    EndDate = DateTime.UtcNow.AddDays(10),
    TimePeriod = TimePeriod.Custom
};

QueryHelper.Run(dateTimeQuery);