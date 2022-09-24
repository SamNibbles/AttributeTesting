using AttributeTesting;

var dateTimeQuery = new DateTimeQuery
{
    StartDate = DateTime.UtcNow,
    EndDate = DateTime.UtcNow.AddDays(10),
    TimePeriod = TimePeriod.Custom
};

var url = QueryHelper.AddQueryToUrl("www.test.com", dateTimeQuery);
Console.WriteLine($"Adding to url test - URL: {url}");
Console.WriteLine();

var testUrl = "www.test.com?startDate=2021-01-03&endDate=2022-02-01&timePeriod=Custom";
var query = (DateTimeQuery)QueryHelper.ExtractQueryFromUrl(testUrl, typeof(DateTimeQuery));

Console.WriteLine($"Extracting from url test - ");
Console.WriteLine($"StartDate: {query.StartDate}");
Console.WriteLine($"EndDate: {query.EndDate}");
Console.WriteLine($"TimePeriod: {query.TimePeriod}");