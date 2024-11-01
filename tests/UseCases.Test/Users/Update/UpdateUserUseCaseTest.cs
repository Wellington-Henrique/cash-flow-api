using CashFlow.Application.UseCases.Users.UpdateProfile;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Users.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var user = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            // Act
            var act = async () => await useCase.Execute(request);

            // Assert
            await act.Should().NotThrowAsync();
            user.Name.Should().Be(request.Name);
            user.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task ErrorNameEmpty()
        {
            // Arrange
            var user = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase(user);

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
            var user = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();
            var useCase = CreateUseCase(user, request.Email);

            // Act
            var act = async () => await useCase.Execute(request);

            // Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
            result.Where(ex => ex.GetErrors.Count == 1 && ex.GetErrors.Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        private UpdateUserUseCase CreateUseCase(User user, string? email = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
            var loggedUser = LoggedUserBuilder.Build(user);
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder();

            if (string.IsNullOrWhiteSpace(email) == false)
            {
                readOnlyRepository.ExistActiveUserWithEmail(email);
            }

            return new UpdateUserUseCase(
                loggedUser,
                readOnlyRepository.Build(),
                updateRepository,
                unitOfWork
            );
        }
    }
}
