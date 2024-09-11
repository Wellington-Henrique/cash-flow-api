using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using FluentAssertions;
using Validators.Tests.Requests;

namespace Validators.Tests.Users.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            // Arrange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public void ErrorNameEmpty(string name)
        {
            // Arrange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = name;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors
                .Should()
                .ContainSingle().And
                .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
        }


        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public void ErrorEmailEmpty(string email)
        {
            // Arrange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = email;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors
                .Should()
                .ContainSingle().And
                .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
        }
    }
}
