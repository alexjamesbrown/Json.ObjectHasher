# JSON Object Hasher

A .NET library that generates deterministic MD5 hashes from objects using JSON serialization.

## Features

- **Deterministic hashing** - Same object values always produce the same hash
- **Property exclusion** - Use `[HashIgnore]` to exclude properties from hash calculation
- **Multiple object support** - Hash multiple objects together in a single call
- **Thread-safe** - Safe to use across multiple threads
- **Customizable serialization** - Pass custom `JsonSerializerSettings` for fine-grained control

## Installation

### NuGet

```
dotnet add package JsonObjectHasher.Newtonsoft
```

### Project Reference

```xml
<PackageReference Include="JsonObjectHasher.Newtonsoft" Version="1.0.0" />
```

## Quick Start

```csharp
using JsonObjectHasher.Newtonsoft;

var hasher = new ObjectHasher();

var user = new User { Id = 1, Name = "Alex" };
var hash = hasher.GenerateHash(user);
// Returns: "A1B2C3D4E5F6..."
```

## Features in Detail

### Ignoring Properties

Use the `[HashIgnore]` attribute to exclude properties from hash calculation. This is useful for timestamps, IDs, or other volatile fields that shouldn't affect the hash.

```csharp
using json_object_hasher;

public class User
{
    [HashIgnore]
    public int Id { get; set; }

    [HashIgnore]
    public DateTime LastModified { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
}

// These will produce the same hash:
var user1 = new User { Id = 1, LastModified = DateTime.Now, Name = "Alex", Email = "alex@example.com" };
var user2 = new User { Id = 99, LastModified = DateTime.MinValue, Name = "Alex", Email = "alex@example.com" };

hasher.GenerateHash(user1) == hasher.GenerateHash(user2) // true
```

Works with records too:

```csharp
public record Product(
    [property: HashIgnore] int Id,
    string Name,
    decimal Price
);
```

### Hashing Multiple Objects

Generate a single hash from multiple objects:

```csharp
var user = new User { Name = "Alex" };
var order = new Order { Total = 99.99m };
var timestamp = DateTime.UtcNow;

var hash = hasher.GenerateHash(user, order, timestamp);
```

### Custom Serialization Settings

Pass custom `JsonSerializerSettings` for specialized serialization needs:

```csharp
var settings = new JsonSerializerSettings
{
    NullValueHandling = NullValueHandling.Ignore,
    DateFormatString = "yyyy-MM-dd"
};

var hasher = new ObjectHasher(settings);
```

> **Note:** The `ContractResolver` will be overwritten to support `[HashIgnore]`. Other settings are preserved.

## Use Cases

- **Change detection** - Detect when an object's relevant data has changed
- **Event deduplication** - Generate idempotency keys for webhooks or message queues
- **Content-based caching** - Create cache keys based on object content
- **Object comparison** - Compare objects without implementing deep equality

## Why MD5?

MD5 is used because:

1. **Speed** - MD5 is fast, which matters when hashing frequently
2. **Sufficient for non-cryptographic use** - This library is for fingerprinting, not security
3. **Compact output** - 32-character hex string is reasonable for keys and comparison

This library is **not** intended for password hashing, digital signatures, or any security-sensitive application.

## API Reference

### ObjectHasher

```csharp
// Default constructor
var hasher = new ObjectHasher();

// With custom settings
var hasher = new ObjectHasher(JsonSerializerSettings settings);

// Generate hash from one or more objects
string hash = hasher.GenerateHash(params object[] values);
```

### HashIgnoreAttribute

```csharp
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class HashIgnoreAttribute : Attribute
```

Apply to properties or fields to exclude them from hash calculation.

## Project Structure

```
src/
  JsonObjectHasher/              # Core library (HashIgnoreAttribute)
  JsonObjectHasher.Newtonsoft/   # Newtonsoft.Json implementation
tests/
  JsonObjectHasher.Newtonsoft.Tests/
```

## License

MIT
