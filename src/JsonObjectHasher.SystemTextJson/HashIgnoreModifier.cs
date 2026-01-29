using System.Text.Json.Serialization.Metadata;

namespace JsonObjectHasher.SystemTextJson;

/// <summary>
/// Provides a modifier for System.Text.Json that excludes properties marked with <see cref="HashIgnoreAttribute"/>.
/// </summary>
/// <remarks>
/// This modifier is used internally by <see cref="ObjectHasher"/> to filter out ignored properties
/// during JSON serialization.
/// </remarks>
internal static class HashIgnoreModifier
{
    /// <summary>
    /// Modifies the JSON type info to exclude properties marked with <see cref="HashIgnoreAttribute"/>.
    /// </summary>
    /// <param name="typeInfo">The JSON type info to modify.</param>
    internal static void ExcludeHashIgnoredProperties(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        foreach (var property in typeInfo.Properties)
        {
            if (property.AttributeProvider?.GetCustomAttributes(typeof(HashIgnoreAttribute), true).Length > 0)
            {
                property.ShouldSerialize = (_, _) => false;
            }
        }
    }
}
