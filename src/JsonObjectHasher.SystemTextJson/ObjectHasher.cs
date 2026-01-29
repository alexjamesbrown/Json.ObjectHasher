using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace JsonObjectHasher.SystemTextJson;

/// <summary>
/// Generates deterministic MD5 hashes from objects using JSON serialization with System.Text.Json.
/// </summary>
/// <remarks>
/// <para>
/// This class serializes objects to JSON and computes an MD5 hash of the result.
/// The same object values will always produce the same hash, making it suitable for
/// change detection, caching keys, and deduplication scenarios.
/// </para>
/// <para>
/// Properties marked with <see cref="HashIgnoreAttribute"/> are excluded
/// from serialization and do not affect the generated hash.
/// </para>
/// <para>
/// This class is thread-safe.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var hasher = new ObjectHasher();
/// var hash = hasher.GenerateHash(new { Name = "Alex", Age = 30 });
/// </code>
/// </example>
public class ObjectHasher : IObjectHasher
{
    private readonly JsonSerializerOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectHasher"/> class with custom serialization options.
    /// </summary>
    /// <param name="options">
    /// The JSON serialization options to use. Note that a custom <see cref="JsonSerializerOptions.TypeInfoResolver"/>
    /// will be configured to support the <see cref="HashIgnoreAttribute"/>.
    /// </param>
    /// <example>
    /// <code>
    /// var options = new JsonSerializerOptions
    /// {
    ///     DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    /// };
    /// var hasher = new ObjectHasher(options);
    /// </code>
    /// </example>
    public ObjectHasher(JsonSerializerOptions options)
    {
        _options = new JsonSerializerOptions(options)
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    HashIgnoreModifier.ExcludeHashIgnoredProperties
                }
            }
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectHasher"/> class with default options.
    /// </summary>
    public ObjectHasher() : this(new JsonSerializerOptions())
    {
    }

    /// <summary>
    /// Generates an MD5 hash from one or more objects.
    /// </summary>
    /// <param name="values">The objects to include in the hash calculation.</param>
    /// <returns>A 32-character uppercase hexadecimal string representing the MD5 hash.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is null.</exception>
    /// <remarks>
    /// Objects are serialized to JSON and the resulting string is hashed using MD5.
    /// Properties marked with <see cref="HashIgnoreAttribute"/> are excluded.
    /// </remarks>
    /// <example>
    /// <code>
    /// var hasher = new ObjectHasher();
    ///
    /// // Single object
    /// var hash1 = hasher.GenerateHash(user);
    ///
    /// // Multiple objects
    /// var hash2 = hasher.GenerateHash(user, order, timestamp);
    /// </code>
    /// </example>
    public string GenerateHash(params object[] values)
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        var textValue = JsonSerializer.Serialize(values, _options);

        using var hasher = MD5.Create();

        var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(textValue));

        return string.Concat(hash.Select(a => a.ToString("X2")));
    }
}
