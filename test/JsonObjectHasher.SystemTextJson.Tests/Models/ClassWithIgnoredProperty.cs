using JsonObjectHasher;

namespace JsonObjectHasher.SystemTextJson.Tests.Models;

public class ClassWithIgnoredProperty
{
    [HashIgnore] public string AccountId { get; set; }

    public string EmailAddress { get; set; }
}
