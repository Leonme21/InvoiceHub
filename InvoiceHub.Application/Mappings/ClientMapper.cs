using InvoiceHub.Application.Dtos.Client;
using InvoiceHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Application.Mappings
{
    public static class ClientMapper
    {
        public static ClientDetailResponseDto ToDetailDto(this Client client)
        {
            return new ClientDetailResponseDto
            {
                Id = client.Id,
                Name = client.Name,
                Document = client.Document,
                Email = client.Email,
                Phone = client.Phone,
                Address = client.Address,
                CreatedAt = client.CreatedAt,
                IsActive = client.IsActive
            };
        }

        public static ClientListResponseDto ToListDto(this Client client)
        {
            return new ClientListResponseDto
            {
                Id = client.Id,
                Name = client.Name,
                Document = client.Document,
                IsActive = client.IsActive
            };
        }

        public static Client ToEntity(this CreateClientDto dto)
        {
            return new Client
            {
                Name = dto.Name,
                Document = dto.Document,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address
            };
        }

        public static void UpdateEntity(this UpdateClientDto dto, Client client)
        {
            client.Name = dto.Name;
            client.Email = dto.Email;
            client.Phone = dto.Phone;
            client.Address = dto.Address;
            client.UpdatedAt = DateTime.Now;
        }
    }
}
