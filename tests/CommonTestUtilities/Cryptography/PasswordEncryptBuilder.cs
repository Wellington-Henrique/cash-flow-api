using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncryptBuilder
    {
        public static IPasswordEncripter Build()
        {
            var mock = new Mock<IPasswordEncripter>();

            mock
                .Setup(passwordEncripter => passwordEncripter
                .Encrypt(It.IsAny<string>()))
                .Returns("asdasd!!@323*");

            return mock.Object;
        }
    }
}
