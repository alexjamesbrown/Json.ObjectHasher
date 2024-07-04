using System.Reflection;
using Newtonsoft.Json;

namespace JsonObjectHasher.Newtonsoft;

public class HashIgnoreJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return true; // Assume it can convert any type
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<HashIgnoreAttribute>() == null)
            {
                var propertyValue = property.GetValue(value);
                writer.WritePropertyName(property.Name);
                serializer.Serialize(writer, propertyValue);
            }
        }
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException("Unnecessary for this example");
    }
}