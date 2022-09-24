using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTesting;

public class QueryAttribute : Attribute
{
    public string QueryName { get; init; }
    public Type Serializer { get; init; }

    public QueryAttribute() { }

    public QueryAttribute(string queryName, Type serializer)
    {
        QueryName = queryName;
        Serializer = serializer;
    }
}
