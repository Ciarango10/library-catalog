using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(category => category.Id);
        builder.Property(category => category.Name).HasMaxLength(100).IsRequired();

        builder.HasMany(category => category.Books)
            .WithOne(book => book.Category)
            .HasForeignKey(book => book.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(category => category.Books)
            .HasField("_books")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasData(
            new { Id = 1, Name = "Tecnología" },
            new { Id = 2, Name = "Ciencias" },
            new { Id = 3, Name = "Literatura" });
    }
}
