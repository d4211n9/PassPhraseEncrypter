using System.Text;
using System.Text.Json;
using ObjectJsonConverter.Interfaces;

namespace ObjectJsonConverter;

public class JsonConverter<T> : IObjectJsonConverter<T>
{
    public string ObjectToJson(T o)
    {
        return JsonSerializer.Serialize(o);
    }

    public T? JsonToObject(string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }
}