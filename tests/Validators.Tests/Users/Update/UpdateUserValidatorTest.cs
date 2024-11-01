using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Application.UseCases.Users.UpdateProfile;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Validators.Tests.Requests;

namespace Validators.Tests.Users.Update
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            // Arrange
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();

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
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = name;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
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
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = email;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors
                .Should()
                .ContainSingle().And
                .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
        }

        [Fact]
        public void ErrorEmailInvalid()
        {
            // Arrange
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "wellingtonhlc.com";

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors
                .Should()
                .ContainSingle().And
                .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
        }
    }
}
