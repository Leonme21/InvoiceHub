using System.Net.Http.Json;
using InvoiceHub.Client.Models;

namespace InvoiceHub.Client.Services
{
    public class InvoiceApiService
    {
        private readonly HttpClient _http;
        private const string BasePath = "api/Invoice";

        public InvoiceApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<InvoiceListResponseDto>> GetByClientIdAsync(
            int clientId)
        {
            return await _http
                       .GetFromJsonAsync<List<InvoiceListResponseDto>>(
                           $"{BasePath}/client/{clientId}")
                   ?? new List<InvoiceListResponseDto>();
        }

        public async Task<List<InvoiceListResponseDto>> GetAllAsync()
        {
            try
            {
                var paged = await GetPagedAsync(
                    null,
                    1,
                    10000);

                return paged.Items
                       ?? new List<InvoiceListResponseDto>();
            }
            catch
            {
                return new List<InvoiceListResponseDto>();
            }
        }

        public async Task<PagedResult<InvoiceListResponseDto>> GetPagedAsync(
            string? search,
            int page = 1,
            int pageSize = 10)
        {
            var query =
                $"?page={page}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(search))
            {
                query +=
                    $"&search={Uri.EscapeDataString(search)}";
            }

            return await _http
                       .GetFromJsonAsync<PagedResult<InvoiceListResponseDto>>(
                           $"{BasePath}{query}")
                   ?? new PagedResult<InvoiceListResponseDto>();
        }

        public async Task<InvoiceResponseDto?> GetByIdAsync(int id)
        {
            return await _http
                .GetFromJsonAsync<InvoiceResponseDto>(
                    $"{BasePath}/{id}");
        }

        public async Task<InvoiceResponseDto?> CreateAsync(
            InvoiceRequestDto dto)
        {
            var response =
                await _http.PostAsJsonAsync(
                    BasePath,
                    dto);

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                throw new ApplicationException(error);
            }

            return await response.Content
                .ReadFromJsonAsync<InvoiceResponseDto>();
        }
    }
}