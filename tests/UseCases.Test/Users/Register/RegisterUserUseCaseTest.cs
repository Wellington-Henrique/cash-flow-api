
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Requests;
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

        private RegisterUserUseCase CreateUseCase()
        {
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeRepository = UseWriteOnlyRepositoryBuilder.Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();
            var passwordEncripter = PasswordEncryptBuilder.Build();

            return new RegisterUserUseCase(
                mapper,
                passwordEncripter,
                null,
                writeRepository, 
                unitOfWork,
                tokenGenerator
            );
        }
    }
}
