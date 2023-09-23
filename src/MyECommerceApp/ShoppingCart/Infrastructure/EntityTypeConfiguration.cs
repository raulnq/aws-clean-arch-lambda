using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyECommerceApp.ShoppingCart.Domain;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;

namespace MyECommerceApp.ShoppingCart.Infrastructure;

public class EntityTypeConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder
            .ToTable(Tables.ShoppingCartItems);

        builder
            .HasKey(s => s.ShoppingCartItemId);

        builder
            .Property(s => s.Quantity)
            .HasColumnType("decimal(19,4)");
    }
}
