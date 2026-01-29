using System.Collections.Concurrent;
using AutoFixture;
using FluentAssertions;
using JsonObjectHasher.Newtonsoft.Tests.Models;
using Xunit;

namespace JsonObjectHasher.Newtonsoft.Tests;

public class ObjectHasherTests
{
    private readonly ObjectHasher _objectHasher = new();

    [Fact]
    public void SameObject_ShouldGetSameHashCode()
    {
        var productUpgradeSelected = new Fixture().Create<AccountUpgradedEvent>();

        var md5Code1 = _objectHasher.GenerateHash(productUpgradeSelected);
        var md5Code2 = _objectHasher.GenerateHash(productUpgradeSelected);

        md5Code1.Should().Be(md5Code2);
    }

    [Fact]
    public void SameObjectWithUrl_ShouldGetSameHashCode()
    {
        var productUpgradeSelected = new Fixture().Create<AccountUpgradedEvent>();
        var url = "http://localhost/webhook";
        var md5Code1 = _objectHasher.GenerateHash(productUpgradeSelected, url);
        var md5Code2 = _objectHasher.GenerateHash(productUpgradeSelected, url);

        md5Code1.Should().Be(md5Code2);
    }

    [Fact]
    public void ObjectAndObjectWithUrl_ShouldNotGetSameHashCode()
    {
        var productUpgradeSelected = new Fixture().Create<AccountUpgradedEvent>();
        var url = "http://localhost/webhook";

        var md5Code1 = _objectHasher.GenerateHash(productUpgradeSelected, url);
        var md5Code2 = _objectHasher.GenerateHash(productUpgradeSelected);

        md5Code1.Should().NotBe(md5Code2);
    }

    [Fact]
    public void DifferentObjects_ShouldNotGetSameHashCode()
    {
        var productUpgradeSelected = new Fixture().Create<AccountUpgradedEvent>();

        var changedValue = productUpgradeSelected with
        {
            DateOfUpgrade = DateTimeOffset.Now
        };

        var md5Code1 = _objectHasher.GenerateHash(productUpgradeSelected);
        var md5Code2 = _objectHasher.GenerateHash(changedValue);

        md5Code1.Should().NotBe(md5Code2);
    }

    [Fact]
    public async Task DifferentObjects_ShouldNotGetSameHashCode_Across_Multiple_Threads()
    {
        var numThreads = 10;

        var hashCodes = new ConcurrentBag<string>();

        var tasks = Enumerable.Range(0, numThreads).Select
        (_ => Task.Run
            (() =>
                {
                    var up = new Fixture().Create<AnotherCompletedEvent>();
                    var down = new Fixture().Create<CompletedEvent>();

                    var md5Code1 = _objectHasher.GenerateHash(up);
                    var md5Code2 = _objectHasher.GenerateHash(down);

                    hashCodes.Add(md5Code1);
                    hashCodes.Add(md5Code2);
                }
            )
        ).ToArray();

        // Wait for all tasks to complete
        await Task.WhenAll(tasks);

        // Ensure that all hash codes in the ConcurrentBag are unique
        hashCodes.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void DifferentObjectTypes_ShouldNotGetSameHashCode()
    {
        var productUpgradeSelected = new Fixture().Create<AccountUpgradedEvent>();
        var productUpgradeDeclined = new Fixture().Create<DeclinedEvent>();

        var md5Code1 = _objectHasher.GenerateHash(productUpgradeSelected);
        var md5Code2 = _objectHasher.GenerateHash(productUpgradeDeclined);

        md5Code1.Should().NotBe(md5Code2);
    }

    [Fact]
    public void DifferentObjectTypesWithSame_ShouldNotGetSameHashCode()
    {
        var url = "http://localhost/webhook";
        var productUpgradeSelected = new Fixture().Create<AccountUpgradedEvent>();
        var productUpgradeDeclined = new Fixture().Create<DeclinedEvent>();

        var md5Code1 = _objectHasher.GenerateHash(productUpgradeSelected, url);
        var md5Code2 = _objectHasher.GenerateHash(productUpgradeDeclined, url);

        md5Code1.Should().NotBe(md5Code2);
    }

    [Fact]
    public void SameObjectButDifferentPropertyInNestedObject_Should_Return_Different_Hashes()
    {
        var productUpgradeSelected1 = new Fixture().Create<AccountUpgradedEvent>();

        var productUpgradeSelected2 = productUpgradeSelected1 with
        {
            Account = new Account("diff1", "diff2")
        };

        var md5Code1 = _objectHasher.GenerateHash(productUpgradeSelected1);
        var md5Code2 = _objectHasher.GenerateHash(productUpgradeSelected2);

        md5Code1.Should().NotBe(md5Code2);
    }

    [Fact]
    public void Does_Not_Include_Properties_Marked_As_Ignored_In_Hash_Value_With_Class()
    {
        var item1 = new ClassWithIgnoredProperty
        {
            AccountId = "1234",
            EmailAddress = "test@test.com"
        };

        var item2 = new
        {
            EmailAddress = "test@test.com"
        };

        var hash1 = _objectHasher.GenerateHash(item1);
        var hash2 = _objectHasher.GenerateHash(item2);

        hash1.Should().Be(hash2);
    }

    [Fact]
    public void Does_Not_Include_Properties_Marked_As_Ignored_In_Hash_Value_With_Record()
    {
        var item1 = new RecordWithIgnoredProperty("1234", "test@test.com");

        var item2 = new
        {
            EmailAddress = "test@test.com"
        };

        var hash1 = _objectHasher.GenerateHash(item1);
        var hash2 = _objectHasher.GenerateHash(item2);

        hash1.Should().Be(hash2);
    }

    [Fact]
    public void Passing_Null_Should_Throw_Exception()
    {
        Assert.Throws<ArgumentNullException>(() => _objectHasher.GenerateHash(null));
    }
}