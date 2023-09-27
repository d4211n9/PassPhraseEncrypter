namespace ObjectJsonConverter.Interfaces;

public interface IObjectJsonConverter<T>
{
    string ObjectToJson(T o);
    T? JsonToObject(string json);
}