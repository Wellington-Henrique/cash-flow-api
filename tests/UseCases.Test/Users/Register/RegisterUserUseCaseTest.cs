
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
            var useCase = CreateUseCase();
            var request = RequestRegisterUserJsonBuilder.Build();

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
            var useCase = CreateUseCase();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            // Act
            var act = async () => await useCase.Execute(request);

            // Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
            result.Where(ex => ex.GetErrors.Count == 1 && ex.GetErrors.Contains(ResourceErrorMessages.NAME_EMPTY));
        }

        private RegisterUserUseCase CreateUseCase()
        {
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeRepository = UseWriteOnlyRepositoryBuilder.Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();
            var passwordEncripter = PasswordEncryptBuilder.Build();
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder().Build();

            return new RegisterUserUseCase(
                mapper,
                passwordEncripter,
                readOnlyRepository,
                writeRepository, 
                unitOfWork,
                tokenGenerator
            );
        }
    }
}
