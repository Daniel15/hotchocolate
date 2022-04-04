using System;
using Xunit;

namespace HotChocolate.Language;

public class EnumValueDefinitionNodeTests
{
    [Fact]
    public void Equals_With_Same_Location()
    {
        var a = new EnumValueDefinitionNode(
            TestLocations.Location1,
            new("aa"),
            null,
            Array.Empty<DirectiveNode>());
        var b = new EnumValueDefinitionNode(
            TestLocations.Location1,
            new("aa"),
            null,
            Array.Empty<DirectiveNode>());
        var c = new EnumValueDefinitionNode(
            TestLocations.Location1,
            new("ab"),
            null,
            Array.Empty<DirectiveNode>());

        // act
        var abResult = a.Equals(b);
        var aaResult = a.Equals(a);
        var acResult = a.Equals(c);
        var aNullResult = a.Equals(default);

        // assert
        Assert.True(abResult);
        Assert.True(aaResult);
        Assert.False(acResult);
        Assert.False(aNullResult);
    }

    [Fact]
    public void Equals_With_Different_Location()
    {
        // arrange
        var a = new EnumValueDefinitionNode(
            TestLocations.Location1,
            new("aa"),
            null,
            Array.Empty<DirectiveNode>());
        var b = new EnumValueDefinitionNode(
            TestLocations.Location2,
            new("aa"),
            null,
            Array.Empty<DirectiveNode>());
        var c = new EnumValueDefinitionNode(
            TestLocations.Location1,
            new("ab"),
            null,
            Array.Empty<DirectiveNode>());

        // act
        var abResult = a.Equals(b);
        var aaResult = a.Equals(a);
        var acResult = a.Equals(c);
        var aNullResult = a.Equals(default);

        // assert
        Assert.True(abResult);
        Assert.True(aaResult);
        Assert.False(acResult);
        Assert.False(aNullResult);
    }

    [Fact]
    public void GetHashCode_With_Location()
    {
        // arrange
        var a = new EnumValueDefinitionNode(
            TestLocations.Location1,
            new("aa"),
            null,
            Array.Empty<DirectiveNode>());
        var b = new EnumValueDefinitionNode(
            TestLocations.Location2,
            new("aa"),
            null,
            Array.Empty<DirectiveNode>());
        var c = new EnumValueDefinitionNode(
            TestLocations.Location1,
            new("ab"),
            null,
            Array.Empty<DirectiveNode>());
        var d = new EnumValueDefinitionNode(
            TestLocations.Location2,
            new("ab"),
            null,
            Array.Empty<DirectiveNode>());

        // act
        var aHash = a.GetHashCode();
        var bHash = b.GetHashCode();
        var cHash = c.GetHashCode();
        var dHash = d.GetHashCode();

        // assert
        Assert.Equal(aHash, bHash);
        Assert.NotEqual(aHash, cHash);
        Assert.Equal(cHash, dHash);
        Assert.NotEqual(aHash, dHash);
    }
}