using BlazorApiDemo.Models;

namespace BlazorApiDemo.Components.Pages;

public partial class Holidays
{
    private int _numberOfYears;
    private string? _errorMessage;
    private List<Holiday>? _holidays = [];
    private Dictionary<string, string?> _countries = [];
    public int Year { get; set; } = DateTime.Now.Year;
    public string CountryCode { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        GetConfigurationData();
        await GetHolidaysData();
    }

    private void GetConfigurationData()
    {
        GetCountriesFromConfiguration();
        GetDefaultCountryFromConfiguration();
        GetNumberOfYearsFromConfiguration();
    }

    private async Task GetHolidaysData()
    {
        try
        {
            _holidays = await _nagerDateClient
                .GetAllHolidaysForCountryByYearAsync(Year, CountryCode);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = "Sorry, there was a problem with processing " +
                "of your request.\nPlease try again later.";
            await Console.Out.WriteLineAsync($"{_errorMessage}\n{ex}");
        }
        catch (TaskCanceledException ex)
        {
            _errorMessage = "Sorry, there was a problem with processing " +
                "of your request.\nRequest timout exceeded." +
                "\nPlease try again later.";
            await Console.Out.WriteLineAsync($"{_errorMessage}\n{ex}");
        }
        catch (ArgumentNullException ex)
        {
            _errorMessage = "Sorry, there was a problem with processing " +
                "of your request.\nCheck request parameters." +
                "\nPlease try again later.";
            await Console.Out.WriteLineAsync($"{_errorMessage}\n{ex}");
        }
    }

    private void GetDefaultCountryFromConfiguration()
    {
        CountryCode = _configuration
            .GetValue<string>("DefaultCountryCode") ?? string.Empty;
    }

    private void GetNumberOfYearsFromConfiguration()
    {
        _numberOfYears = _configuration
            .GetValue<int>("NumberOfYears");
    }

    private void GetCountriesFromConfiguration()
    {
        _countries = _configuration
            .GetSection("Countries")
            .GetChildren()
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private async Task ProcessSelection()
    {
        await GetHolidaysData();
    }
}