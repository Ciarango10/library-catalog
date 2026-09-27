using Library.Domain.Entities;
using Library.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Persistence.Configurations;

public sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(book => book.Id);

        builder.Property(book => book.Title).HasMaxLength(200).IsRequired();
        builder.Property(book => book.Isbn)
            .HasConversion(isbn => isbn.Value, value => new Isbn(value))
            .HasMaxLength(13)
            .IsUnicode(false)
            .IsRequired();
        builder.HasIndex(book => book.Isbn).IsUnique();

        builder.Property(book => book.PublicationYear)
            .HasConversion(year => year.Value, value => new PublicationYear(value))
            .IsRequired();

        builder.Property(book => book.AuthorId).IsRequired();
        builder.Property(book => book.CategoryId).IsRequired();

        builder.HasData(
            Seed(1, "Arquitectura de software", "9780000000019", 2018, 1, 1),
            Seed(2, "Fundamentos de bases de datos", "9780000000026", 2016, 2, 1),
            Seed(3, "Redes distribuidas", "9780000000033", 2020, 3, 1),
            Seed(4, "Patrones de diseño", "9780000000040", 2014, 4, 1),
            Seed(5, "Historia de la ciencia", "9780000000057", 2012, 5, 2),
            Seed(6, "Álgebra lineal aplicada", "9780000000064", 2019, 1, 2),
            Seed(7, "Cálculo para ingeniería", "9780000000071", 2017, 2, 2),
            Seed(8, "Literatura latinoamericana", "9780000000088", 2015, 3, 3),
            Seed(9, "Narrativas contemporáneas", "9780000000095", 2021, 4, 3),
            Seed(10, "Poesía y memoria", "9780000000101", 2013, 5, 3));
    }

    private static object Seed(
        int id, string title, string isbn, int publicationYear, int authorId, int categoryId) =>
        new
        {
            Id = id,
            Title = title,
            Isbn = new Isbn(isbn),
            PublicationYear = new PublicationYear(publicationYear),
            AuthorId = authorId,
            CategoryId = categoryId
        };
}
