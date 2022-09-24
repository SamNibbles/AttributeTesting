using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTesting;

public static class QueryHelper
{
    private const string SerializeMethodName = "SerializeQueryParameter";
    private const string DeserializeMethodName = "DeserializeQueryParameter";

    public static void Run(BaseQuery query)
    {
        var queryProcessors = GetQueryProcessors(query);

        TestQueryParameterExtraction(typeof(DateTimeQuery), queryProcessors);
        TestQueryParameterInsertion(query, queryProcessors);
    }

    private static void TestQueryParameterExtraction(Type queryType, List<QueryProcessor> queryProcessors)
    {
        var testUrl = new UriBuilder("www.test.com?startDate=2021-01-02&endDate=2022-02-01&timePeriod=Custom");
        var queryDict = QueryHelpers.ParseQuery(testUrl.Query);

        var queryInstance = Activator.CreateInstance(queryType);
        foreach (var processor in queryProcessors)
        {
            var queryParameter = queryDict[processor.QueryName].First();
            var deserialiserPropertyValue = processor.Serializer.GetMethod(DeserializeMethodName).Invoke(null, new[] { queryParameter });
            processor.Property.SetValue(queryInstance, deserialiserPropertyValue);
        }

        var instanceType = queryInstance.GetType();
        var propertyValues = instanceType.GetProperties().Select(property => $"{property.Name}: {property.GetValue(queryInstance)}");

        Console.WriteLine($"Parameter extraction test - Instance type: {instanceType}, Property values: {string.Join(", ", propertyValues)}");
    }

    private static void TestQueryParameterInsertion(BaseQuery query, List<QueryProcessor> queryProcessors)
    {
        var queryDict = new Dictionary<string, string>();
        foreach (var processor in queryProcessors)
        {
            var propertyValue = processor.Property.GetValue(query);
            var serialiserPropertyValue = (string)processor.Serializer.GetMethod(SerializeMethodName).Invoke(null, new[] { propertyValue });
            queryDict.Add(processor.QueryName, serialiserPropertyValue);
        }

        var testUrl = QueryHelpers.AddQueryString("www.test.com", queryDict);
        Console.WriteLine($"Parameter insertion test - URL: {testUrl}");
    }

    private static List<QueryProcessor> GetQueryProcessors(BaseQuery query)
    {
        var queryProperties = query.GetType().GetProperties();
        return queryProperties.Select(property =>
        {
            var queryAttribute = property.GetCustomAttribute<QueryAttribute>();
            return new QueryProcessor
            {
                Property = property,
                QueryName = queryAttribute.QueryName,
                Serializer = queryAttribute.Serializer
            };
        }).ToList();
    }

    private static Type[] GetAllQueryClasses()
    {
        return typeof(Program).Assembly.GetTypes().Where(type => type.GetCustomAttribute<QueryAttribute>() is not null).ToArray();
    }

    private class QueryProcessor
    {
        public PropertyInfo Property { get; set; }
        public string QueryName { get; set; }
        public Type Serializer { get; set; }
    }
}