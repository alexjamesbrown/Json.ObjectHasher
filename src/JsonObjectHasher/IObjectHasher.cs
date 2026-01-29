namespace JsonObjectHasher;

/// <summary>
/// Defines a contract for generating deterministic hashes from objects.
/// </summary>
/// <remarks>
/// <para>
/// Implementations serialize objects to JSON and compute a hash of the result.
/// The same object values will always produce the same hash, making it suitable for
/// change detection, caching keys, and deduplication scenarios.
/// </para>
/// <para>
/// Properties marked with <see cref="HashIgnoreAttribute"/> are excluded
/// from serialization and do not affect the generated hash.
/// </para>
/// </remarks>
public interface IObjectHasher
{
    /// <summary>
    /// Generates a hash from one or more objects.
    /// </summary>
    /// <param name="values">The objects to include in the hash calculation.</param>
    /// <returns>A string representing the computed hash.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is null.</exception>
    /// <remarks>
    /// Objects are serialized to JSON and the resulting string is hashed.
    /// Properties marked with <see cref="HashIgnoreAttribute"/> are excluded.
    /// </remarks>
    /// <example>
    /// <code>
    /// IObjectHasher hasher = new ObjectHasher();
    ///
    /// // Single object
    /// var hash1 = hasher.GenerateHash(user);
    ///
    /// // Multiple objects
    /// var hash2 = hasher.GenerateHash(user, order, timestamp);
    /// </code>
    /// </example>
    string GenerateHash(params object[] values);
}
