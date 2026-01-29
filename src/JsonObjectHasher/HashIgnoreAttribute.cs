namespace JsonObjectHasher;

/// <summary>
/// Marks a property or field to be excluded from hash calculation.
/// </summary>
/// <remarks>
/// Use this attribute on properties or fields that should not affect the generated hash,
/// such as database IDs, timestamps, or other volatile values.
/// </remarks>
/// <example>
/// <code>
/// public class User
/// {
///     [HashIgnore]
///     public int Id { get; set; }
///
///     [HashIgnore]
///     public DateTime LastModified { get; set; }
///
///     public string Name { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class HashIgnoreAttribute : Attribute;
