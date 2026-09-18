using InvoiceHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Domain.Entities
{
    public class InvoiceDetail : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }
}
