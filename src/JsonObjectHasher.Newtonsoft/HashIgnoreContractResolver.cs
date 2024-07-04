using System.Reflection;
using json_object_hasher;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonObjectHasher.Newtonsoft;

public class HashIgnoreContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (member.GetCustomAttribute<HashIgnoreAttribute>() != null)
            property.ShouldSerialize = _ => false;

        return property;
    }
}