using JsonObjectHasher;

namespace JsonObjectHasher.SystemTextJson.Tests.Models;

public record RecordWithIgnoredProperty([property: HashIgnore] string AccountId, string EmailAddress);
