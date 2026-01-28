using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace JsonObjectHasher.Newtonsoft;

/// <summary>
/// Generates deterministic MD5 hashes from objects using JSON serialization.
/// </summary>
/// <remarks>
/// <para>
/// This class serializes objects to JSON and computes an MD5 hash of the result.
/// The same object values will always produce the same hash, making it suitable for
/// change detection, caching keys, and deduplication scenarios.
/// </para>
/// <para>
/// Properties marked with <see cref="json_object_hasher.HashIgnoreAttribute"/> are excluded
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
public class ObjectHasher
{
    private readonly JsonSerializerSettings _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectHasher"/> class with custom serialization settings.
    /// </summary>
    /// <param name="settings">
    /// The JSON serialization settings to use. Note that the <see cref="JsonSerializerSettings.ContractResolver"/>
    /// will be overwritten to support the <see cref="json_object_hasher.HashIgnoreAttribute"/>.
    /// </param>
    /// <example>
    /// <code>
    /// var settings = new JsonSerializerSettings
    /// {
    ///     NullValueHandling = NullValueHandling.Ignore
    /// };
    /// var hasher = new ObjectHasher(settings);
    /// </code>
    /// </example>
    public ObjectHasher(JsonSerializerSettings settings)
    {
        _settings = new JsonSerializerSettings(settings)
        {
            ContractResolver = new HashIgnoreContractResolver()
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectHasher"/> class with default settings.
    /// </summary>
    public ObjectHasher() : this(new JsonSerializerSettings())
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
    /// Properties marked with <see cref="json_object_hasher.HashIgnoreAttribute"/> are excluded.
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

        var textValue = JsonConvert
            .SerializeObject(values, _settings);

        using var hasher = MD5.Create();

        var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(textValue));

        return string.Concat(hash.Select(a => a.ToString("X2")));
    }
}
