using System.Net.Http.Json;
using InvoiceHub.Client.Models;

namespace InvoiceHub.Client.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _http;
        private const string BasePath = "api/Product";

        public ProductApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProductListResponseDto>> GetAllAsync()
        {
            try
            {
                var paged = await GetPagedAsync(
                    null,
                    1,
                    10000,
                    true);

                return paged.Items
                       ?? new List<ProductListResponseDto>();
            }
            catch
            {
                return new List<ProductListResponseDto>();
            }
        }

        public async Task<PagedResult<ProductListResponseDto>> GetPagedAsync(
            string? search,
            int page = 1,
            int pageSize = 10,
            bool? isActive = true)
        {
            var query =
                $"?page={page}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(search))
            {
                query +=
                    $"&search={Uri.EscapeDataString(search)}";
            }

            if (isActive.HasValue)
            {
                query +=
                    $"&isActive={isActive.Value.ToString().ToLower()}";
            }

            return await _http
                       .GetFromJsonAsync<PagedResult<ProductListResponseDto>>(
                           $"{BasePath}{query}")
                   ?? new PagedResult<ProductListResponseDto>();
        }

        public async Task<ProductDetailResponseDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ProductDetailResponseDto>($"{BasePath}/{id}");
        }

        public async Task<ProductDetailResponseDto?> CreateAsync(CreateProductDto dto)
        {
            var response = await _http.PostAsJsonAsync(BasePath, dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(error);
            }
            return await response.Content.ReadFromJsonAsync<ProductDetailResponseDto>();
        }

        public async Task<ProductDetailResponseDto?> UpdateAsync(int id, UpdateProductDto dto)
        {
            var response = await _http.PutAsJsonAsync($"{BasePath}/{id}", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(error);
            }
            return await response.Content.ReadFromJsonAsync<ProductDetailResponseDto>();
        }

        public async Task DeleteAsync(int id)
        {
            await ChangeStatusAsync(id, false);
        }

        public async Task ChangeStatusAsync(
            int id,
            bool isActive)
        {
            var dto = new ChangeProductStatusDto
            {
                IsActive = isActive
            };

            var response =
                await _http.PatchAsJsonAsync(
                    $"{BasePath}/{id}/status",
                    dto);

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                throw new ApplicationException(
                    string.IsNullOrWhiteSpace(error)
                        ? "No fue posible actualizar el estado del producto."
                        : error);
            }
        }

    }
}
