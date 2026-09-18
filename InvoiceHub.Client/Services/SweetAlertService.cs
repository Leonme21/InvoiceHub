using Microsoft.JSInterop;

namespace InvoiceHub.Client.Services
{
    public class SweetAlertService
    {
        private readonly IJSRuntime _jsRuntime;

        public SweetAlertService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ShowSuccessToastAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("sweetAlertInterop.showToast", "success", message);
        }

        public async Task ShowErrorToastAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("sweetAlertInterop.showToast", "error", message);
        }

        public async Task<bool> ConfirmAsync(string title, string text, string confirmText = "Sí, continuar")
        {
            return await _jsRuntime.InvokeAsync<bool>("sweetAlertInterop.confirm", title, text, confirmText);
        }
    }
}
