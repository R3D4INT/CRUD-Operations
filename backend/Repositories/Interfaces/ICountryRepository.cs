using backend.Dtos.Request;


namespace backend.Repositories.Interfaces;

public interface ICountryRepository
{
    Task ImportCountriesFromExcelAsync(string filePath);
    Task<List<CountryRequest>> GetCountriesAsync();
    Task<CountryRequest> GetCountryByNameAsync(string name);
}