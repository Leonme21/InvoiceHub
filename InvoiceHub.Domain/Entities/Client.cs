using InvoiceHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Domain.Entities
{
    public class Client : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string? Email { get; set; }
        public long? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
