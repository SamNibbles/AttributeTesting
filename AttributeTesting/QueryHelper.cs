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

        TestQueryParameterExtraction(query, queryProcessors);
        TestQueryParameterInsertion(query, queryProcessors);
    }

    private static void TestQueryParameterExtraction(BaseQuery query, List<QueryProcessor> queryProcessors)
    {

    }

    private static void TestQueryParameterInsertion(BaseQuery query, List<QueryProcessor> queryProcessors)
    {
        var queryDict = new Dictionary<string, string>();
        foreach (var processor in queryProcessors)
        {
            var propertyValue = processor.Property.GetValue(query);
            var deserialiserPropertyValue = (string)processor.Serializer.GetMethod(DeserializeMethodName).Invoke(null, new[] { propertyValue });
            queryDict.Add(processor.QueryName, deserialiserPropertyValue);
        }

        var testUrl = QueryHelpers.AddQueryString("www.test.com", queryDict);
        Console.WriteLine($"Test URL: {testUrl}");
    }

    private static List<QueryProcessor> GetQueryProcessors(BaseQuery query)
    {
        var queryProperties = query.GetType().GetProperties();
        return queryProperties.Select(property =>
        {
            var queryAttribute = property.GetCustomAttributes(false).OfType<QueryAttribute>().Single();
            return new QueryProcessor
            {
                Property = property,
                QueryName = queryAttribute.QueryName,
                Serializer = queryAttribute.Serializer
            };
        }).ToList();
    }

    private class QueryProcessor
    {
        public PropertyInfo Property { get; set; }
        public string QueryName { get; set; }
        public Type Serializer { get; set; }
    }
}