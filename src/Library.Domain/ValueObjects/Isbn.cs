using Library.Domain.Common;
using Library.Domain.Exceptions;

namespace Library.Domain.ValueObjects;

public sealed class Isbn : ValueObject
{
    public string Value { get; }

    public Isbn(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("El ISBN es obligatorio.");

        var normalized = value.Replace("-", "").Replace(" ", "").ToUpperInvariant();

        if (!IsValidIsbn10(normalized) && !IsValidIsbn13(normalized))
            throw new DomainException($"'{value}' no es un ISBN-10 ni un ISBN-13 válido.");

        Value = normalized;
    }

    private static bool IsValidIsbn10(string isbn)
    {
        if (isbn.Length != 10)
            return false;

        var sum = 0;
        for (var i = 0; i < 10; i++)
        {
            int digit;
            if (char.IsAsciiDigit(isbn[i]))
                digit = isbn[i] - '0';
            else if (i == 9 && isbn[i] == 'X')
                digit = 10;
            else
                return false;

            sum += digit * (10 - i);
        }

        return sum % 11 == 0;
    }

    private static bool IsValidIsbn13(string isbn)
    {
        if (isbn.Length != 13)
            return false;

        var sum = 0;
        for (var i = 0; i < 13; i++)
        {
            if (!char.IsAsciiDigit(isbn[i]))
                return false;

            sum += (isbn[i] - '0') * (i % 2 == 0 ? 1 : 3);
        }

        return sum % 10 == 0;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
