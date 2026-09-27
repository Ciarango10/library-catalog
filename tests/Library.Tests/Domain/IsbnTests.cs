using Library.Domain.Exceptions;
using Library.Domain.ValueObjects;

namespace Library.Tests.Domain;

public class IsbnTests
{
    [Theory]
    [InlineData("0306406152")]
    [InlineData("0-306-40615-2")]
    [InlineData("080442957X")]
    [InlineData("9780306406157")]
    [InlineData("978-0-306-40615-7")]
    [InlineData("9780132350884")]
    public void Constructor_WithValidIsbn_CreatesInstance(string value)
    {
        var isbn = new Isbn(value);

        Assert.False(string.IsNullOrEmpty(isbn.Value));
    }

    [Theory]
    [InlineData("0-306-40615-2", "0306406152")]
    [InlineData("978-0-306-40615-7", "9780306406157")]
    [InlineData("080442957x", "080442957X")]
    [InlineData("  978 0 306 40615 7  ", "9780306406157")]
    public void Constructor_NormalizesSeparatorsAndCase(string input, string expected)
    {
        Assert.Equal(expected, new Isbn(input).Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123")]
    [InlineData("0306406153")]
    [InlineData("9780306406158")]
    [InlineData("ABCDEFGHIJ")]
    [InlineData("X306406152")]
    [InlineData("97803064061XX")]
    [InlineData("03064061523")]
    public void Constructor_WithInvalidIsbn_ThrowsDomainException(string? value)
    {
        Assert.Throws<DomainException>(() => new Isbn(value!));
    }

    [Fact]
    public void Equality_SameValue_AreEqualEvenWithDifferentFormatting()
    {
        var a = new Isbn("978-0-306-40615-7");
        var b = new Isbn("9780306406157");

        Assert.NotSame(a, b);
        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.False(a != b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equality_DifferentValue_AreNotEqual()
    {
        var a = new Isbn("9780306406157");
        var b = new Isbn("9780132350884");

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void Equality_WithNull_IsFalse()
    {
        var isbn = new Isbn("9780306406157");

        Assert.False(isbn.Equals(null));
        Assert.False(isbn == null);
    }

    [Fact]
    public void ToString_ReturnsNormalizedValue()
    {
        Assert.Equal("9780306406157", new Isbn("978-0-306-40615-7").ToString());
    }
}
