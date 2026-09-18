using InvoiceHub.Application.Dtos.Client;
using InvoiceHub.Application.Dtos.Invoice;
using InvoiceHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Application.Mappings
{
    public static class InvoiceMapper
    {
        public static InvoiceDetailResponseDto ToDetailDto(this InvoiceDetail invoiceDetail)
        {
            return new InvoiceDetailResponseDto
            {
                Id = invoiceDetail.Id,
                ProductId = invoiceDetail.ProductId,
                Quantity = invoiceDetail.Quantity,
                ProductName = invoiceDetail.Product.Name ?? string.Empty,
                UnitPrice = invoiceDetail.UnitPrice,
                LineTotal = invoiceDetail.LineTotal
            };
        }

        public static InvoiceListResponseDto ToListDto(this Invoice invoice)
        {
            return new InvoiceListResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                ClientName = invoice.Client?.Name ?? string.Empty,
                Total = invoice.Total
            };
        }

        public static InvoiceResponseDto ToDto(this Invoice invoice)
        {
            return new InvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                ClientId = invoice.ClientId,
                ClientName = invoice.Client.Name ?? string.Empty,
                SubTotal = invoice.SubTotal,
                TaxPercentage = invoice.TaxPercentage,
                TaxAmount = invoice.TaxAmount,
                Total = invoice.Total,
                Observations = invoice.Observations,
                Details = invoice.InvoiceDetails.Select(d => d.ToDetailDto()).ToList()
            };
        }

        public static InvoiceDetail ToEntityInvoiceDetail(this InvoiceDetailRequestDto dto)
        {
            return new InvoiceDetail
            {
                Quantity = dto.Quantity,
                ProductId = dto.ProductId
            };
        }

        public static Invoice ToEntityInvoice(this InvoiceRequestDto dto)
        {
            return new Invoice
            {
                ClientId = dto.ClientId,
                Observations = dto.Observations,
                InvoiceDetails = dto.Details.Select(d => d.ToEntityInvoiceDetail()).ToList()
            };
        }
    }
}
