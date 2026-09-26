using Library.Domain.Exceptions;
using Library.Domain.ValueObjects;

namespace Library.Tests.Domain;

public class PublicationYearTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(1605)]
    [InlineData(1999)]
    public void Constructor_WithPastYear_CreatesInstance(int year)
    {
        Assert.Equal(year, new PublicationYear(year).Value);
    }

    [Fact]
    public void Constructor_WithCurrentYear_CreatesInstance()
    {
        var currentYear = DateTime.UtcNow.Year;

        Assert.Equal(currentYear, new PublicationYear(currentYear).Value);
    }

    [Fact]
    public void Constructor_WithFutureYear_ThrowsDomainException()
    {
        var nextYear = DateTime.UtcNow.Year + 1;

        Assert.Throws<DomainException>(() => new PublicationYear(nextYear));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-2000)]
    [InlineData(int.MinValue)]
    public void Constructor_WithNegativeYear_ThrowsDomainException(int year)
    {
        Assert.Throws<DomainException>(() => new PublicationYear(year));
    }

    [Fact]
    public void Equality_SameYear_AreEqualByValue()
    {
        var a = new PublicationYear(2008);
        var b = new PublicationYear(2008);

        Assert.NotSame(a, b);
        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.False(a != b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equality_DifferentYear_AreNotEqual()
    {
        Assert.NotEqual(new PublicationYear(2008), new PublicationYear(2009));
        Assert.True(new PublicationYear(2008) != new PublicationYear(2009));
    }

    [Fact]
    public void Equality_DifferentValueObjectTypes_AreNotEqual()
    {
        Assert.False(new PublicationYear(2008).Equals(new Isbn("9780306406157")));
    }
}
