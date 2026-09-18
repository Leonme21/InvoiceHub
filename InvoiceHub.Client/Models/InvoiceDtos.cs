namespace InvoiceHub.Client.Models
{

    public class InvoiceRequestDto
    {
        public int ClientId { get; set; }
        public string? Observations { get; set; }
        public List<InvoiceDetailRequestDto> Details { get; set; } = new();
    }

    public class InvoiceDetailRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class InvoiceResponseDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public string? Observations { get; set; }
        public List<InvoiceDetailResponseDto> Details { get; set; } = new();
    }

    public class InvoiceListResponseDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }

    public class InvoiceDetailResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int AvailableStock { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
