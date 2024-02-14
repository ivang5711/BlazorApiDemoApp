using BlazorApiDemo.Models;

namespace BlazorApiDemo.Services
{
    public interface INagerDateClient
    {
        Task<List<Holiday>?> GetAllHolidaysForCountryByYearAsync(int year, string countryCode);
    }
}