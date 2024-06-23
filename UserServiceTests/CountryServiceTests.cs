using backend.Models;
using backend.Models.enums;
using backend.Repositories.Interfaces;
using backend.Services.Implementations;
using backend.UnitOfWork.Interfaces;
using Moq;
using OfficeOpenXml;

namespace UserServiceTests
{
    public class CountryServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICountryRepository> _mockCountryRepository;
        private CountryService _countryService;
        private List<Country> _importedCountries;

        public CountryServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCountryRepository = new Mock<ICountryRepository>();
            _mockUnitOfWork.Setup(uow => uow.countryRepository).Returns(_mockCountryRepository.Object);
            _importedCountries = new List<Country>();
            _countryService = new CountryService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task ImportCountriesFromExcel_ShouldImportCallSaveChanges()
        {
            var filePath = "test_countries.xlsx";
            CreateTestExcelFile(filePath);

            await _countryService.ImportCountriesFromExcelAsync(filePath);

            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public async Task ImportCountriesFromExcel_ShouldImportImportMethod()
        {
            var filePath = "test_countries.xlsx";
            CreateTestExcelFile(filePath);

            await _countryService.ImportCountriesFromExcelAsync(filePath);

            _mockCountryRepository.Verify(repo => repo.ImportCountriesFromExcelAsync(filePath), Times.Once);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public async Task ImportCountriesFromExcel_ShouldImportCountriesCorrectly()
        {
            var filePath = "test_countries.xlsx";
            CreateTestExcelFile(filePath);

            _mockCountryRepository
                .Setup(repo => repo.ImportCountriesFromExcelAsync(It.IsAny<string>()))
                .Callback<string>(path => { _importedCountries = ReadCountriesFromExcel(path); })
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _countryService.ImportCountriesFromExcelAsync(filePath);

            _mockCountryRepository.Verify(repo => repo.ImportCountriesFromExcelAsync(filePath), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);

            Assert.Equal(3, _importedCountries.Count);
            Assert.Contains(_importedCountries,
                c => c is { Id: 1, Name: "Ukraine", Region: Region.Europe, Population: 10000 });
            Assert.Contains(_importedCountries,
                c => c is { Id: 2, Name: "Germany", Region: Region.Europe, Population: 666666 });
            Assert.Contains(_importedCountries,
                c => c is { Id: 3, Name: "USA", Region: Region.NorthAmerica, Population: 5654654 });

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public async Task ImportCountriesFromExcel_ShouldHandleEmptyFile()
        {
            var filePath = "empty_test_countries.xlsx";
            CreateEmptyTestExcelFile(filePath);

            _mockCountryRepository
                .Setup(repo => repo.ImportCountriesFromExcelAsync(It.IsAny<string>()))
                .Callback<string>(path => { _importedCountries = ReadCountriesFromExcel(path); })
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _countryService.ImportCountriesFromExcelAsync(filePath);

            _mockCountryRepository.Verify(repo => repo.ImportCountriesFromExcelAsync(filePath), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);

            Assert.Empty(_importedCountries);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }


        private void CreateTestExcelFile(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Countries");
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Population";
                worksheet.Cells[1, 4].Value = "Region";

                var countries = new List<Country> {
                    new Country {Id = 1, Name = "Ukraine", Region = Region.Europe, Population = 10000},
                    new Country {Id = 2, Name = "Germany", Region = Region.Europe, Population = 666666},
                    new Country {Id = 3, Name = "USA", Region = Region.NorthAmerica, Population = 5654654}
                };

                for (var i = 0; i < countries.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = countries[i].Id;
                    worksheet.Cells[i + 2, 2].Value = countries[i].Name;
                    worksheet.Cells[i + 2, 3].Value = countries[i].Population;
                    worksheet.Cells[i + 2, 4].Value = countries[i].Region.ToString();
                }

                package.SaveAs(new FileInfo(filePath));
            }
        }

        private List<Country> ReadCountriesFromExcel(string filePath)
        {
            var countries = new List<Country>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var country = new Country
                    {
                        Id = Convert.ToInt32(worksheet.Cells[row, 1].Value),
                        Name = worksheet.Cells[row, 2].Value.ToString(),
                        Population = Convert.ToInt32(worksheet.Cells[row, 3].Value),
                        Region = Enum.TryParse(worksheet.Cells[row, 4].Value.ToString(), out Region region) ? region : Region.Unknown
                    };
                    countries.Add(country);
                }
            }

            return countries;
        }

        private void CreateEmptyTestExcelFile(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Countries");
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Population";
                worksheet.Cells[1, 4].Value = "Region";

                package.SaveAs(new FileInfo(filePath));
            }
        }
    }
}
