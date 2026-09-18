using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Application.Dtos.Invoice
{
    public class InvoiceRequestDto
    {
        public int ClientId { get; set; }
        public string? Observations { get; set; }
        public List<InvoiceDetailRequestDto> Details { get; set; } = new List<InvoiceDetailRequestDto>();
    }
}
