using backend.Dtos.Request;

namespace backend.Services.Interfaces
{
    public interface ICountryService
    {
        Task ImportCountriesFromExcelAsync(string filePath);
        Task<List<CountryRequest>> GetCountriesAsync();
        Task<CountryRequest> GetCountryByNameAsync(string name);
    }
}
