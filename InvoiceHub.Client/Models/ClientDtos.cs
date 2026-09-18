namespace InvoiceHub.Client.Models
{
    public class ClientListResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class ClientDetailResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string? Email { get; set; }
        public long? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string? Email { get; set; }
        public long? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public long? Phone { get; set; }
        public string? Address { get; set; }
    }
}
