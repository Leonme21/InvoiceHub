using System.Net.Http.Json;
using InvoiceHub.Client.Models;

namespace InvoiceHub.Client.Services
{
    public class ClientApiService
    {
        private readonly HttpClient _http;
        private const string BasePath = "api/Client";

        public ClientApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ClientListResponseDto>> GetAllAsync()
        {
            try
            {
                // Para el POS queremos únicamente clientes activos.
                var paged = await GetPagedAsync(
                    null,
                    1,
                    10000,
                    true);

                return paged.Items ?? new List<ClientListResponseDto>();
            }
            catch
            {
                return new List<ClientListResponseDto>();
            }
        }

        public async Task<PagedResult<ClientListResponseDto>> GetPagedAsync(
            string? search,
            int page = 1,
            int pageSize = 10,
            bool? isActive = true)
        {
            var query = $"?page={page}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(search))
            {
                query += $"&search={Uri.EscapeDataString(search)}";
            }

            if (isActive.HasValue)
            {
                query += $"&isActive={isActive.Value.ToString().ToLower()}";
            }

            return await _http
                       .GetFromJsonAsync<PagedResult<ClientListResponseDto>>(
                           $"{BasePath}{query}")
                   ?? new PagedResult<ClientListResponseDto>();
        }

        public async Task<ClientDetailResponseDto?> GetByIdAsync(int id)
        {
            return await _http
                .GetFromJsonAsync<ClientDetailResponseDto>(
                    $"{BasePath}/{id}");
        }

        public async Task<ClientDetailResponseDto?> CreateAsync(
            CreateClientDto dto)
        {
            var response = await _http.PostAsJsonAsync(
                BasePath,
                dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new ApplicationException(error);
            }

            return await response.Content
                .ReadFromJsonAsync<ClientDetailResponseDto>();
        }

        public async Task<ClientDetailResponseDto?> UpdateAsync(
            int id,
            UpdateClientDto dto)
        {
            var response = await _http.PutAsJsonAsync(
                $"{BasePath}/{id}",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new ApplicationException(error);
            }

            return await response.Content
                .ReadFromJsonAsync<ClientDetailResponseDto>();
        }

        public async Task ChangeStatusAsync(
            int id,
            bool isActive)
        {
            var dto = new ChangeClientStatusDto
            {
                IsActive = isActive
            };

            var response = await _http.PatchAsJsonAsync(
                $"{BasePath}/{id}/status",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new ApplicationException(
                    string.IsNullOrWhiteSpace(error)
                        ? "No fue posible actualizar el estado del cliente."
                        : error);
            }
        }


        public async Task<List<InvoiceListResponseDto>> GetByClientIdAsync(
            int clientId)
        {
            return await _http.GetFromJsonAsync<List<InvoiceListResponseDto>>(
                       $"api/Invoice/client/{clientId}")
                   ?? new List<InvoiceListResponseDto>();
        }


        public async Task DeleteAsync(int id)
        {
            await ChangeStatusAsync(id, false);
        }
    }
}