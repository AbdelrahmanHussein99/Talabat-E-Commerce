using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(O => O.Status)
                .HasConversion(Ostatus => Ostatus.ToString(), Ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), Ostatus));


            builder.OwnsOne(O => O.ShippingAddress, Sa => Sa.WithOwner());

            builder.HasOne(O => O.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
