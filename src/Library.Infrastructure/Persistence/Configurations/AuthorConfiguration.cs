using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Persistence.Configurations;

public sealed class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(author => author.Id);
        builder.Property(author => author.Name).HasMaxLength(150).IsRequired();

        builder.HasMany(author => author.Books)
            .WithOne(book => book.Author)
            .HasForeignKey(book => book.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(author => author.Books)
            .HasField("_books")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasData(
            new { Id = 1, Name = "Ana Torres" },
            new { Id = 2, Name = "Carlos Méndez" },
            new { Id = 3, Name = "Laura Ramírez" },
            new { Id = 4, Name = "Diego Vargas" },
            new { Id = 5, Name = "Sofía Herrera" });
    }
}
