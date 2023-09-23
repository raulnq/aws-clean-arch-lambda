using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyECommerceApp.Products.Domain;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;

namespace MyECommerceApp.Products.Infrastructure;

public class EntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .ToTable(Tables.Products);

        builder
            .HasKey(p => p.ProductId);

        builder
            .Property(p => p.Price)
            .HasColumnType("decimal(19,4)");
    }
}
