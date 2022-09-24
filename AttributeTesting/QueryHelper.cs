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

    public static BaseQuery ExtractQueryFromUrl(string url, Type queryType)
    {
        var queryProcessors = GetQueryProcessors(queryType);

        var testUrl = new UriBuilder(url);
        var queryDict = QueryHelpers.ParseQuery(testUrl.Query);

        var queryInstance = Activator.CreateInstance(queryType);
        foreach (var processor in queryProcessors)
        {
            var queryParameter = queryDict[processor.QueryName].First();
            var deserialiserPropertyValue = processor.Serializer.GetMethod(DeserializeMethodName).Invoke(null, new[] { queryParameter });
            processor.Property.SetValue(queryInstance, deserialiserPropertyValue);
        }

        return (BaseQuery)queryInstance;
    }

    public static string AddQueryToUrl(string baseUrl, BaseQuery query)
    {
        var queryDict = new Dictionary<string, string>();
        var queryProcessors = GetQueryProcessors(query.GetType());
        foreach (var processor in queryProcessors)
        {
            var propertyValue = processor.Property.GetValue(query);
            var serialiserPropertyValue = (string)processor.Serializer.GetMethod(SerializeMethodName).Invoke(null, new[] { propertyValue });
            queryDict.Add(processor.QueryName, serialiserPropertyValue);
        }

        return QueryHelpers.AddQueryString(baseUrl, queryDict);
    }

    private static List<QueryProcessor> GetQueryProcessors(Type queryType)
    {
        var queryProperties = queryType.GetProperties();
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

    private class QueryProcessor
    {
        public PropertyInfo Property { get; set; }
        public string QueryName { get; set; }
        public Type Serializer { get; set; }
    }
}