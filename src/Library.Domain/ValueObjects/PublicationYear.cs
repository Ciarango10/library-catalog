using Library.Domain.Common;
using Library.Domain.Exceptions;

namespace Library.Domain.ValueObjects;

public sealed class PublicationYear : ValueObject
{
    public int Value { get; }

    public PublicationYear(int value)
    {
        if (value < 0)
            throw new DomainException("El año de publicación no puede ser negativo.");

        var currentYear = DateTime.UtcNow.Year;
        if (value > currentYear)
            throw new DomainException($"El año de publicación no puede ser futuro (máximo {currentYear}).");

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
