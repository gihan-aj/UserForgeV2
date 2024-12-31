using Application.Abstractions.Services;
using Application.Users.Commands.Register;
using Domain.Users;
using FluentAssertions;
using Moq;
using SharedKernal;

namespace Application.UnitTests.Users.Commands
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IEmailService> _mailServiceMock;

        public RegisterUserCommandHandlerTests()
        {
            _userServiceMock = new();
            _mailServiceMock = new();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenEmailIsNotUnique()
        {
            // Arrange
            var command = new RegisterUserCommand("first", "last", "email@test.com", null, null, "Password@123");

            _userServiceMock.Setup(
                x => x.CreateAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string?>(),
                    It.IsAny<DateOnly?>(),
                    It.IsAny<string>()))
                .ReturnsAsync(Result.Failure<User>(UserErrors.Conflict.EmailAlreadyExists("email@test.com")));


            var handler = new RegisterUserCommandHandler(
                _userServiceMock.Object,
                _mailServiceMock.Object);

            // Act
            Result<string> result = await handler.Handle(command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.Conflict.EmailAlreadyExists("email@test.com"));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenUserIsCreated()
        {
            // Arrange
            var command = new RegisterUserCommand("first", "last", "email@test.com", null, null, "Password@123");

            var user = new User
            {
                Id = "123"
            };

            _userServiceMock.Setup(
                x => x.CreateAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string?>(),
                    It.IsAny<DateOnly?>(),
                    It.IsAny<string>()))
                .ReturnsAsync(Result.Success<User>(user));

            _userServiceMock.Setup(
                x => x.GenerateEmailConfirmationTokenAsync(
                    It.IsAny<User>()))
                .ReturnsAsync(Result.Success<string>(It.IsAny<string>()));

            _mailServiceMock.Setup(
                x => x.SendConfirmationEmailAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                .ReturnsAsync(Result.Success<User>(user));

            var handler = new RegisterUserCommandHandler(
                _userServiceMock.Object,
                _mailServiceMock.Object);

            // Act
            Result<string> result = await handler.Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_Should_CallGenerateEmailConfirmationTokenAsync_WhenUserIsCreated()
        {
            // Arrange
            var command = new RegisterUserCommand("first", "last", "email@test.com", null, null, "Password@123");

            var user = new User
            {
                Id = "123"
            };

            _userServiceMock.Setup(
                x => x.CreateAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string?>(),
                    It.IsAny<DateOnly?>(),
                    It.IsAny<string>()))
                .ReturnsAsync(Result.Success<User>(user));

            _userServiceMock.Setup(
                x => x.GenerateEmailConfirmationTokenAsync(
                    It.IsAny<User>()))
                .ReturnsAsync(Result.Success<string>(It.IsAny<string>()));

            _mailServiceMock.Setup(
                x => x.SendConfirmationEmailAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                .ReturnsAsync(Result.Success<User>(user));

            var handler = new RegisterUserCommandHandler(
                _userServiceMock.Object,
                _mailServiceMock.Object);

            // Act
            Result<string> result = await handler.Handle(command, default);

            // Assert
            _userServiceMock.Verify(
                x => x.GenerateEmailConfirmationTokenAsync(It.Is<User>(u => u.Id ==  user.Id)),
                Times.Once());
        }

        [Fact]
        public async Task Handle_Should_NotCallSendEmailConfirmationAsync_WhenEmailIsNotUnique()
        {
            // Arrange
            var command = new RegisterUserCommand("first", "last", "email@test.com", null, null, "Password@123");

            _userServiceMock.Setup(
                x => x.CreateAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string?>(),
                    It.IsAny<DateOnly?>(),
                    It.IsAny<string>()))
                .ReturnsAsync(Result.Failure<User>(UserErrors.Conflict.EmailAlreadyExists("email@test.com")));

            var handler = new RegisterUserCommandHandler(
                _userServiceMock.Object,
                _mailServiceMock.Object);

            // Act
            await handler.Handle(command, default);

            // Assert
            _mailServiceMock.Verify(
                x => x.SendConfirmationEmailAsync(It.IsAny<User>(), It.IsAny<string>()),
                Times.Never);
        }
    }
}
