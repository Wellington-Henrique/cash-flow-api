
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Token;
using FluentAssertions;
using Validators.Tests.Requests;

namespace UseCases.Test.Users.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase();

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task ErrorNameEmpty()
        {
            // Arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase();
            request.Name = string.Empty;

            // Act
            var act = async () => await useCase.Execute(request);

            // Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
            result.Where(ex => ex.GetErrors.Count == 1 && ex.GetErrors.Contains(ResourceErrorMessages.NAME_EMPTY));
        }

        [Fact]
        public async Task ErrorEmailAlreadyExist()
        {
            // Arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase(request.Email);

            // Act
            var act = async () => await useCase.Execute(request);

            // Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
            result.Where(ex => ex.GetErrors.Count == 1 && ex.GetErrors.Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        private RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();
            var passwordEncripter = new PasswordEncrypterBuilder().Build();
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder();

            if (string.IsNullOrWhiteSpace(email) == false)
            {
                readOnlyRepository.ExistActiveUserWithEmail(email);
            }

            return new RegisterUserUseCase(
                mapper,
                passwordEncripter,
                readOnlyRepository.Build(),
                writeRepository, 
                unitOfWork,
                tokenGenerator
            );
        }
    }
}
