using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UseWriteOnlyRepositoryBuilder
    {
        public static IUserWriteOnlyRepository Build()
        {
            var mock = new Mock<IUserWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
