using backend.DAL;
using backend.Repositories.Interfaces;
using backend.UnitOfWork.Implementations;
using FluentAssertions;
using Moq;

namespace UserServiceTests
{
    public class UnitOfWorkTests
    {
        private readonly Mock<AppDBContext> _mockDbContext;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            _mockDbContext = new Mock<AppDBContext>();
            _mockUserRepository = new Mock<IUserRepository>();
            _unitOfWork = new UnitOfWork(_mockDbContext.Object, _mockUserRepository.Object);
        }

        [Fact]
        public void Constructor_InitializesRepositories()
        {
            Assert.NotNull(_unitOfWork.userRepository);
        }

        [Fact]
        public async Task SaveChangesAsync_CallsDbContextSaveChangesAsync()
        {
            _mockDbContext
                .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            await _unitOfWork.SaveChangesAsync();

            _mockDbContext
                .Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Dispose_CallsDbContextDispose()
        {
            _unitOfWork.Dispose();

            _mockDbContext.Verify(db => db.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            _unitOfWork.Dispose();
            _unitOfWork.Dispose();

            _mockDbContext.Verify(db => db.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_SetsDisposedFlag()
        {
            _unitOfWork.Dispose();

            var disposedField = typeof(UnitOfWork).GetField("disposed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var disposedValue = (bool)disposedField.GetValue(_unitOfWork);

            Assert.True(disposedValue);
        }

        [Fact]
        public void SaveChangesAsync_CanBeCalledAfterDispose()
        {
            _unitOfWork.Dispose();

            var saveChangesAsync = async () => await _unitOfWork.SaveChangesAsync();
            saveChangesAsync.Should().NotThrowAsync();
        }
    }
}
