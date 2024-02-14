using BlazorApiDemo.Models;

namespace BlazorApiDemo.Services
{
    public class NagerDateClient : INagerDateClient
    {
        private readonly HttpClient _httpClient;

        public NagerDateClient(IConfiguration configuration)
        {
            string baseAddress = configuration
                .GetValue<string>("BasePublicHolidaysAddress") ?? string.Empty;
            _httpClient = new()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<List<Holiday>?> GetAllHolidaysForCountryByYearAsync(
            int year, string countryCode)
        {
            string requestUri = $"{year}/{countryCode}";
            return await _httpClient
                .GetFromJsonAsync<List<Holiday>>(
                requestUri: requestUri);
        }
    }
}