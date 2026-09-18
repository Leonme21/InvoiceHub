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
    public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
    {
        public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
        {
            builder.ToTable("InvoiceDetails");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Quantity)
                   .IsRequired();
            
            builder.Property(x => x.UnitPrice)
                   .HasColumnType("decimal(18,2)");
            builder.Property(x => x.LineTotal)
                   .HasColumnType("decimal(18,2)");
            
            builder.HasOne(x => x.Invoice)                 
                   .WithMany(i => i.InvoiceDetails)        
                   .HasForeignKey(x => x.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade);      
                                                           
            builder.HasOne(x => x.Product)                 
                   .WithMany(p => p.InvoiceDetails)        
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
