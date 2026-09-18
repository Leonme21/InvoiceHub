using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Application.Dtos.Invoice
{
    public class InvoiceDetailRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
