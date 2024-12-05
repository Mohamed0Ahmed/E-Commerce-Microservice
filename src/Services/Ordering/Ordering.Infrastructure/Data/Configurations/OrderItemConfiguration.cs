using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> orderItem)
    {
        orderItem.HasKey(oi => oi.Id);

        orderItem.Property(oi => oi.Id).HasConversion(
                                   orderItemId => orderItemId.Value,
                                   dbId => OrderItemId.Of(dbId));

        orderItem.HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        orderItem.Property(oi => oi.Quantity).IsRequired();

        orderItem.Property(oi => oi.Price).IsRequired();
    }
}
