namespace JsonObjectHasher.Newtonsoft.Tests.Models;

public record RecordWithIgnoredProperty([property: HashIgnore] string AccountId, string EmailAddress);