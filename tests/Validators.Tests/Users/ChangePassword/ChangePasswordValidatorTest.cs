using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.ChangePassword
{
    public class ChangePasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            // Arrange
            var validator = new ChangePasswordValidator();
            
            var request = RequestChangePasswordJsonBuilder.Build();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void ErrorNewPasswordEmpty(string password)
        {
            // Arrange
            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = password;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));
        }
    }
}
