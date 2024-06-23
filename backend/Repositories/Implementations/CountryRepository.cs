using AutoMapper;
using backend.DAL;
using backend.Dtos.Request;
using backend.Models;
using backend.Models.enums;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace backend.Repositories.Implementations
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public CountryRepository(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task ImportCountriesFromExcelAsync(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (var row = 2; row <= rowCount; row++)
                {
                    var name = worksheet.Cells[row, 2].Value.ToString();
                    if (!_context.Countries.Any(c => c.Name == name))
                    {
                        var country = new Country
                        {
                            Name = name,
                            Population = Convert.ToInt32(worksheet.Cells[row, 3].Value),
                            Region = Enum.TryParse(worksheet.Cells[row, 4].Value.ToString(), out Region region) ? region : Region.Unknown
                        };
                        _context.Countries.Add(country);
                    }
                }
            }
        }

        public async Task<List<CountryRequest>> GetCountriesAsync()
        {
            var countries = await _context.Countries.ToListAsync();
            var countryRequests = _mapper.Map<List<CountryRequest>>(countries);
            return countryRequests;
        }

        public async Task<CountryRequest> GetCountryByNameAsync(string name)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(e => e.Name == name);
            var countryRequest = _mapper.Map<CountryRequest>(country);
            return countryRequest;
        }
    }
}
