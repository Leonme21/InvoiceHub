using InvoiceHub.Application.Dtos.Product;
using InvoiceHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Application.Mappings
{
    public static class ProductMapper
    {
        public static ProductDetailResponseDto ToDetailDto(this Product product)
        {
            return new ProductDetailResponseDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description ?? string.Empty, 
                UnitPrice = product.UnitPrice,
                Stock = product.Stock,
                IsActive = product.IsActive
            };
        }

        public static ProductListResponseDto ToListDto(
            this Product product)
        {
            return new ProductListResponseDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                UnitPrice = product.UnitPrice,
                Stock = product.Stock,
                IsActive = product.IsActive
            };
        }

        public static Product ToEntity(this CreateProductDto dto)
        {
            return new Product
            {
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                UnitPrice = dto.UnitPrice,
                Stock = dto.Stock
            };
        }

        public static void UpdateEntity(this UpdateProductDto dto, Product product)
        {
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.UnitPrice = dto.UnitPrice;
            product.Stock = dto.Stock;
            product.UpdatedAt = DateTime.Now;
        }
    }
}
