using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Application.Dtos.Client
{
    public class CreateClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty; 
        public string? Email { get; set; }
        public long? Phone { get; set; }
        public string? Address { get; set; }
    }
}
