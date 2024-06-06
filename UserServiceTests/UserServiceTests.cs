using AutoMapper;
using backend;
using backend.Dtos.Request;
using backend.Helpers;
using backend.Models;
using backend.Models.enums;
using backend.Repositories.Interfaces;
using backend.Services.Implementations;
using backend.UnitOfWork.Interfaces;
using Moq;
using System.Linq.Expressions;

namespace UserServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryAndSaveChanges()
        {
            var userRequest = new UserRequest
            {
                Id = 1,
                Gender = Gender.Female,
                Name = "Baobab",
                Surname = "Test",
                Address = "Germany",
                Age = 19,
                Email = "test@gmail.com"
            };
            var user = new User
            {
                Id = 1,
                Gender = Gender.Female,
                Name = "Baobab",
                Surname = "Test",
                Address = "Germany",
                Age = 19,
                Email = "test@gmail.com"
            };

            _mapperMock.Setup(m => m.Map<User>(userRequest)).Returns(user);
            _userRepositoryMock.Setup(r => r.AddAsync(userRequest))
                .ReturnsAsync(Result<UserRequest>.Success(userRequest));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);


            await _userService.AddAsync(userRequest);

            _userRepositoryMock.Verify(r => r.AddAsync(userRequest), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowExceptionOnRepositoryError()
        {
            var userRequest = new UserRequest
            {
                Id = 1,
                Gender = Gender.Female,
                Name = "Baobab",
                Surname = "Test",
                Address = "Germany",
                Age = 19,
                Email = "test@gmail.com"
            };
            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserRequest>()))
                .ReturnsAsync(Result<UserRequest>.Fail(RepositoryMessages.FailedAddItemMessage));

            await Assert.ThrowsAsync<NullReferenceException>(() => _userService.AddAsync(userRequest));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUserAndSaveChanges()
        {
            var userRequest = new UserRequest
            {
                Id = 1,
                Gender = Gender.Female,
                Name = "Baobab",
                Surname = "Test",
                Address = "Germany",
                Age = 19,
                Email = "test@gmail.com"
            };

            var user = new User
            {
                Id = 1,
                Gender = Gender.Female,
                Name = "Baobab",
                Surname = "Test",
                Address = "Germany",
                Age = 19,
                Email = "test@gmail.com"
            };

            _mapperMock.Setup(m => m.Map<User>(userRequest)).Returns(user);
            _mapperMock.Setup(m => m.Map<Expression<Func<User, bool>>>(It.IsAny<Expression<Func<UserRequest, bool>>>())).Returns(e => e.Id == userRequest.Id);

            _userRepositoryMock.Setup(r => r.UpdateAsync(userRequest, It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<bool>.Success(true));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            await _userService.UpdateAsync(userRequest, e => e.Id == userRequest.Id);

            _userRepositoryMock.Verify(r => r.UpdateAsync(userRequest, It.IsAny<Expression<Func<UserRequest, bool>>>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowExceptionWhenUserNotFound()
        {
            var userRequest = new UserRequest
            {
                Id = 1,
                Gender = Gender.Female,
                Name = "Baobab",
                Surname = "Test",
                Address = "Germany",
                Age = 19,
                Email = "test@gmail.com"
            };


            _mapperMock.Setup(m => m.Map<Expression<Func<User, bool>>>(It.IsAny<Expression<Func<UserRequest, bool>>>())).Returns(e => e.Id == userRequest.Id);
            _userRepositoryMock.Setup(r => r.UpdateAsync(userRequest, It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<bool>.Fail(RepositoryMessages.FailedUpdateItemMessage));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            await Assert.ThrowsAsync<Exception>(() => _userService.UpdateAsync(userRequest, e => e.Id == userRequest.Id));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteUserAndSaveChanges()
        {
            Expression<Func<UserRequest, bool>> userRequestCondition = u => u.Id == 1;

            var user = new User
            {
                Id = 1,
                Gender = Gender.Female,
                Name = "Baobab",
                Surname = "Test",
                Address = "Germany",
                Age = 19,
                Email = "test@gmail.com"
            };

            var users = new List<User> { user };

            _mapperMock.Setup(m => m.Map<Expression<Func<User, bool>>>(userRequestCondition))
                .Returns(e => e.Id == 1);

            _userRepositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<bool>.Success(true));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            await _userService.DeleteAsync(userRequestCondition);

            _userRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowExceptionWhenUserNotFound()
        {
            Expression<Func<UserRequest, bool>> userRequestCondition = u => u.Id == 1;

            _mapperMock.Setup(m => m.Map<Expression<Func<User, bool>>>(userRequestCondition))
                .Returns(e => e.Id == 1);

            _userRepositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<bool>.Fail(RepositoryMessages.FailedDeleteItemMessage));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.DeleteAsync(userRequestCondition));

            Assert.Equal(ServiceMessages.FailedToDelete + " " + nameof(User) + ".", exception.Message);
        }

        [Fact]
        public async Task GetListByConditionAsync_ShouldReturnUserList()
        {
            Expression<Func<UserRequest, bool>> predicate = u => u.Age > 18;

            var userRequests = new List<UserRequest>
            {
                new UserRequest { Id = 1, Gender = Gender.Female, Name = "Baobab", Surname = "Test", Address = "Germany", Age = 19, Email = "test1@gmail.com" },
                new UserRequest { Id = 2, Gender = Gender.Male, Name = "Acacia", Surname = "Test", Address = "France", Age = 20, Email = "test2@gmail.com" }
            };

            _mapperMock.Setup(m => m.Map<Expression<Func<User, bool>>>(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .Returns((Expression<Func<UserRequest, bool>> sourcePredicate) => u => sourcePredicate.Compile()(new UserRequest { Age = u.Age }));

            _mapperMock.Setup(m => m.Map<IEnumerable<UserRequest>>(It.IsAny<IEnumerable<User>>()))
                .Returns(userRequests);

            _userRepositoryMock.Setup(r => r.GetListByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<IEnumerable<UserRequest>>.Success(userRequests));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var result = await _userService.GetListByConditionAsync(predicate);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _userRepositoryMock.Verify(r => r.GetListByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetListByConditionAsync_ShouldThrowExceptionOnRepositoryError()
        {
            Expression<Func<UserRequest, bool>> predicate = u => u.Age > 18;
            var errorMessage = "Failed to get user list";

            _userRepositoryMock.Setup(r => r.GetListByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<IEnumerable<UserRequest>>.Fail(errorMessage));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.GetListByConditionAsync(predicate));

            Assert.Equal($"{ServiceMessages.FailedGetList} Users.", exception.Message);
        }

        [Fact]
        public async Task GetSingleByConditionAsync_ShouldReturnUser()
        {
            Expression<Func<UserRequest, bool>> predicate = u => u.Id == 1;

            var userRequest = new UserRequest
            {
                Id = 1,
                Gender = Gender.Female,
                Name = "Baobab",
                Surname = "Test",
                Address = "Germany",
                Age = 19,
                Email = "test@gmail.com"
            };

            _userRepositoryMock.Setup(r => r.GetSingleByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<UserRequest>.Success(userRequest));
            

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var result = await _userService.GetSingleByConditionAsync(predicate);

            Assert.NotNull(result);
            Assert.Equal(userRequest.Id, result.Id);
            _userRepositoryMock.Verify(r => r.GetSingleByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetSingleByConditionAsync_ShouldThrowExceptionOnRepositoryError()
        {
            Expression<Func<UserRequest, bool>> predicate = u => u.Id == 1;
            var errorMessage = "Failed to get user";

            _userRepositoryMock.Setup(r => r.GetSingleByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<UserRequest>.Fail(errorMessage));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.GetSingleByConditionAsync(predicate));

            Assert.Equal($"{ServiceMessages.FailedGetSingle} User.", exception.Message);
        }

        [Fact]
        public async Task DeleteUsersOlderThanThirty_ShouldDeleteUsersAndSaveChanges()
        {
            var users = new List<User>
            {
                new User { Id = 1, Age = 31 },
                new User { Id = 2, Age = 29 }
            };

            _userRepositoryMock.Setup(r => r.DeleteUsersOlderThan30Async())
                .ReturnsAsync(Result<bool>.Success(true));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            await _userService.DeleteUsersOlderThanThirty();

            _userRepositoryMock.Verify(r => r.DeleteUsersOlderThan30Async(), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUsersOlderThanThirty_ShouldThrowExceptionOnRepositoryError()
        {
            var errorMessage = "Failed to delete users older than 30";

            _userRepositoryMock.Setup(r => r.DeleteUsersOlderThan30Async())
                .ReturnsAsync(Result<bool>.Fail(errorMessage));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.DeleteUsersOlderThanThirty());

            Assert.Equal($"{ServiceMessages.FailedToDeleteUsersOlderThanThirty} {nameof(User)}", exception.Message);
        }
    }
}