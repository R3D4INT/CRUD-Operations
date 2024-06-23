using backend.Helpers;
using backend.Services.Interfaces;
using Quartz;

namespace backend.BackGroundJob
{
    public class ImportCountriesFromExcelJob : IJob
    {
        private readonly ICountryService _countryServicee;

        public ImportCountriesFromExcelJob(ICountryService countryService)
        {
            _countryServicee = countryService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _countryServicee.ImportCountriesFromExcelAsync(JobStrings.PathForTheExcelFile);
            }
            catch (Exception ex)
            {
                throw new Exception($"{JobStrings.FailedToAdCountriesFromExcel} {ex.Message}");
            }
        }
    }
}
