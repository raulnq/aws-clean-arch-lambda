using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyECommerceApp.ClientRequests.Domain;
using MyECommerceApp.Shared.Infrastructure;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;

namespace MyECommerceApp.ClientRequests.Infrastructure;

public class EntityTypeConfiguration : IEntityTypeConfiguration<ClientRequest>
{
    public void Configure(EntityTypeBuilder<ClientRequest> builder)
    {
        builder
            .ToTable(Tables.ClientRequests);

        builder
            .HasKey(cr => cr.ClientRequestId);

        builder
            .Property(cr => cr.Status)
            .HasConversion(status => status.ToString(), value =>value.ToEnum<ClientRequestStatus>());
    }
}
