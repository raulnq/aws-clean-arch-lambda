using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyECommerceApp.Clients.Domain;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;

namespace MyECommerceApp.Clients.Infrastructure;

public class EntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder
            .ToTable(Tables.Clients);

        builder
            .HasKey(cr => cr.ClientId);
    }
}
