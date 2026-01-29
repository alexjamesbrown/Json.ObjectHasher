using System.Reflection;
using JsonObjectHasher;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonObjectHasher.Newtonsoft;

/// <summary>
/// A JSON.NET contract resolver that excludes properties marked with <see cref="HashIgnoreAttribute"/>.
/// </summary>
/// <remarks>
/// This resolver is used internally by <see cref="ObjectHasher"/> to filter out ignored properties
/// during JSON serialization.
/// </remarks>
public class HashIgnoreContractResolver : DefaultContractResolver
{
    /// <inheritdoc />
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (member.GetCustomAttribute<HashIgnoreAttribute>() != null)
            property.ShouldSerialize = _ => false;

        return property;
    }
}
