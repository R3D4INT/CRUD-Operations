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
using System;
using System.Data.Common;
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

        #region AddAsyncTests

        [Fact]
        public async Task AddAsync_ShouldCallRepository()
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
        }
        [Fact]
        public async Task AddAsync_ShouldCallSaveChanges()
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

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenRepositoryReturnsFailureResult()
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
            }; var errorMessage = "Failed to get user list";

            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserRequest>()))
                .ReturnsAsync(Result<UserRequest>.Fail(errorMessage));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            await Assert.ThrowsAsync<Exception>(() => _userService.AddAsync(userRequest));
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenRepositoryThrowsException()
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

            _userRepositoryMock.Setup(r => r.AddAsync(userRequest))
                .ThrowsAsync(new Exception($"{ServiceMessages.FailedToAdd} {nameof(User)}"));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.AddAsync(userRequest));
            Assert.Equal($"{ServiceMessages.FailedToAdd} {nameof(User)}", exception.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowArgumentNullException_WhenUserRequestIsNull()
        {
            _userRepositoryMock.Setup(r => r.AddAsync(null))
                .ThrowsAsync(new ArgumentNullException(nameof(User), ServiceMessages.FailedToAdd));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.AddAsync(null));
        }

        #endregion

        #region UpdateAsyncTests

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
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
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallSaveChanges()
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

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException()
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

        #endregion

        #region DeleteAsyncTests

        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser()
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
        public async Task DeleteAsync_Should—allSaveChanges()
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

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException()
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

        #endregion

        #region GetListByConditionAsync

        [Fact]
        public async Task GetListByConditionAsync_ShouldReturnEmptyListOfThereAreNoUsers()
        {
            Expression<Func<UserRequest, bool>> predicate = u => u.Age > 28;
            var userRequests = new List<UserRequest>
            {
                new UserRequest { Id = 1, Gender = Gender.Female, Name = "Baobab", Surname = "Test", Address = "Germany", Age = 19, Email = "test1@gmail.com" },
                new UserRequest { Id = 2, Gender = Gender.Male, Name = "Acacia", Surname = "Test", Address = "France", Age = 20, Email = "test2@gmail.com" }
            };

            _mapperMock.Setup(m => m.Map<Expression<Func<User, bool>>>(predicate))
                .Returns(u => u.Age > 28); 

            _mapperMock.Setup(m => m.Map<IEnumerable<UserRequest>>(It.IsAny<IEnumerable<User>>()))
                .Returns(userRequests);

            _userRepositoryMock.Setup(r => r.GetListByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<IEnumerable<UserRequest>>.Success(new List<UserRequest>())); 

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var _userService = new UserService(_mapperMock.Object, _unitOfWorkMock.Object);

            var result = await _userService.GetListByConditionAsync(predicate);

            Assert.NotNull(result);
            Assert.Empty(result);
            _userRepositoryMock.Verify(r => r.GetListByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetListByConditionAsync_ShouldReturnSingleUser()
        {
            Expression<Func<UserRequest, bool>> predicate = u => u.Age > 20;
            var userRequests = new List<UserRequest>
            {
                new UserRequest { Id = 1, Gender = Gender.Female, Name = "Baobab", Surname = "Test", Address = "Germany", Age = 15, Email = "test1@gmail.com" },
                new UserRequest { Id = 2, Gender = Gender.Male, Name = "Acacia", Surname = "Test", Address = "France", Age = 25, Email = "test2@gmail.com" }
            };

            _mapperMock.Setup(m => m.Map<Expression<Func<User, bool>>>(predicate))
                .Returns(u => u.Age > 20); 

            _mapperMock.Setup(m => m.Map<IEnumerable<UserRequest>>(It.IsAny<IEnumerable<User>>()))
                .Returns(userRequests);

            _userRepositoryMock.Setup(r => r.GetListByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<IEnumerable<UserRequest>>.Success(new List<UserRequest> { userRequests[1] })); 

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var _userService = new UserService(_mapperMock.Object, _unitOfWorkMock.Object);

            var result = await _userService.GetListByConditionAsync(predicate);

            Assert.NotNull(result);
            Assert.Single(result); 
            Assert.Equal(25, result.Single().Age); 
            _userRepositoryMock.Verify(r => r.GetListByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()), Times.Once);
        }
        [Fact]
        public async Task GetListByConditionAsync_ShouldThrowExceptionOnRepositoryError()
        {
            Expression<Func<UserRequest, bool>> predicate = u => u.Age > 18;

                _userRepositoryMock.Setup(r => r.GetListByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                    .ReturnsAsync(Result<IEnumerable<UserRequest>>.Fail(ServiceMessages.FailedGetList));

                _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.GetListByConditionAsync(predicate));

            Assert.Equal($"{ServiceMessages.FailedGetList} Users.", exception.Message);
        }

        #endregion

        #region GetSingleByConditionAsync

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

            var resultWrapper = Result<UserRequest>.Success(userRequest);

            _userRepositoryMock
                .Setup(r => r.GetSingleByConditionAsync(It.Is<Expression<Func<UserRequest, bool>>>(expr => expr.Compile()(userRequest))))
                .ReturnsAsync(resultWrapper);

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var _userService = new UserService(_mapperMock.Object, _unitOfWorkMock.Object);

            var result = await _userService.GetSingleByConditionAsync(predicate);

            Assert.NotNull(result);
            Assert.Equal(userRequest.Id, result.Id);
            _userRepositoryMock.Verify(r => r.GetSingleByConditionAsync(It.Is<Expression<Func<UserRequest, bool>>>(expr => expr.Compile()(userRequest))), Times.Once);
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
        public async Task GetSingleByConditionAsync_ShouldThrowExceptionWhenNoMatch()
        {
            Expression<Func<UserRequest, bool>> predicate = u => u.Id == 999; 

            _userRepositoryMock.Setup(r => r.GetSingleByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ReturnsAsync(Result<UserRequest>.Fail(RepositoryMessages.FailedGetSingleItemMessage));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.GetSingleByConditionAsync(predicate));

            Assert.Equal($"{ServiceMessages.FailedGetSingle} {nameof(User)}.", exception.Message);
        }

        [Fact]
        public async Task GetSingleByConditionAsync_ShouldThrowExceptionOnRepositoryException()
        {
            Expression<Func<UserRequest, bool>> predicate = u => u.Id == 1;

            _userRepositoryMock.Setup(r => r.GetSingleByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ThrowsAsync(new ArgumentException(ServiceMessages.FailedGetSingle));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetSingleByConditionAsync(predicate));

            Assert.Equal($"{ServiceMessages.FailedGetSingle}", exception.Message);
        }

        [Fact]
        public async Task GetSingleByConditionAsync_ShouldThrowExceptionForNullPredicate()
        {
            Expression<Func<UserRequest, bool>> predicate = null;

            _userRepositoryMock
                .Setup(r => r.GetSingleByConditionAsync(It.IsAny<Expression<Func<UserRequest, bool>>>()))
                .ThrowsAsync(new ArgumentNullException(nameof(predicate)));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.GetSingleByConditionAsync(predicate));

            Assert.Equal("predicate", exception.ParamName);
        }
        #endregion

        #region DeleteUsersOlderThanThiryTests

        [Fact]
        public async Task DeleteUsersOlderThanThirty_ShouldDeleteUsersAndSaveChanges()
        {
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

        [Fact]
        public async Task DeleteUsersOlderThanThirty_ShouldNotSaveChanges_WhenNoUsersOlderThanThirty()
        {
            _userRepositoryMock.Setup(r => r.DeleteUsersOlderThan30Async())
                .ReturnsAsync(Result<bool>.Fail(RepositoryMessages.NoUsersOlderThanThirty));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.DeleteUsersOlderThanThirty());

            Assert.Equal($"{ServiceMessages.FailedToDeleteUsersOlderThanThirty} {nameof(User)}", exception.Message);
            _userRepositoryMock.Verify(r => r.DeleteUsersOlderThan30Async(), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteUsersOlderThanThirty_ShouldThrowException_WhenSaveChangesFails()
        {
            _userRepositoryMock.Setup(r => r.DeleteUsersOlderThan30Async())
                .ReturnsAsync(Result<bool>.Success(true));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ThrowsAsync(new Exception("Save failed"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.DeleteUsersOlderThanThirty());

            Assert.Equal("Save failed", exception.Message);
            _userRepositoryMock.Verify(r => r.DeleteUsersOlderThan30Async(), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUsersOlderThanThirty_ShouldReturnSuccess_WhenNoUsersToDelete()
        {
            _userRepositoryMock.Setup(r => r.DeleteUsersOlderThan30Async())
                .ReturnsAsync(Result<bool>.Success(false)); 

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            await _userService.DeleteUsersOlderThanThirty();

            _userRepositoryMock.Verify(r => r.DeleteUsersOlderThan30Async(), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUsersOlderThanThirty_ShouldThrowException_WhenRepositoryReturnsNull()
        {
            _userRepositoryMock
                .Setup(r => r.DeleteUsersOlderThan30Async())
                .ThrowsAsync(new Exception($"{ServiceMessages.FailedToDeleteUsersOlderThanThirty} {nameof(User)}"));

            _unitOfWorkMock.Setup(u => u.userRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.DeleteUsersOlderThanThirty());

            Assert.Equal($"{ServiceMessages.FailedToDeleteUsersOlderThanThirty} {nameof(User)}", exception.Message);
            _userRepositoryMock.Verify(r => r.DeleteUsersOlderThan30Async(), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
        #endregion
    }
}