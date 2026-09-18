using InvoiceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Infrastructure.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Document).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.Document).IsUnique();
            builder.Property(x => x.Email).HasMaxLength(150);
            builder.Property(x => x.Phone);
            builder.Property(x => x.Address).HasMaxLength(250);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.HasQueryFilter(x => x.IsActive);

        }
    }
}
