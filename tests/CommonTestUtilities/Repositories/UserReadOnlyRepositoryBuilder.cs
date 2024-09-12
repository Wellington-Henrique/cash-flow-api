using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUserReadOnlyRepository> _repository;

        public UserReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IUserReadOnlyRepository>();
        }

        public void ExistActiveUserWithEmail(string email)
        {
            // Valor que pode ser usado na chamada de ExistActiveUserWithEmail It.IsAny<string>()
            _repository.Setup(userReadOnly => userReadOnly
                                .ExistActiveUserWithEmail(email)
                        ).ReturnsAsync(true);
        }

        public IUserReadOnlyRepository Build() => _repository.Object;
    }
}
