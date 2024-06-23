using backend.Dtos.Request;
using backend.Services.Interfaces;
using backend.UnitOfWork.Interfaces;

namespace backend.Services.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ImportCountriesFromExcelAsync(string filePath)
        {
            await _unitOfWork.countryRepository.ImportCountriesFromExcelAsync(filePath);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<CountryRequest>> GetCountriesAsync()
        {
            return await _unitOfWork.countryRepository.GetCountriesAsync();
        }

        public async Task<CountryRequest> GetCountryByNameAsync(string name)
        {
            return await  _unitOfWork.countryRepository.GetCountryByNameAsync(name);
        }
    }
}
