using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using FluentAssertions;
using Validators.Tests.Requests;

namespace Validators.Tests.Expenses
{
    public class ExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            //Arrange
            //Criar instâncias necessárias para a criação do teste
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Build();

            //Act
            //Ação, executar o método que será testado
            var result = validator.Validate(request);

            //Assert
            //Resultado do teste, verdadeiro ou falso
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ErrorTitleEmpty()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Build();

            request.Title = string.Empty;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
        }

        [Fact]
        public void ErrorFutureDate()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Build();

            request.Date = DateTime.UtcNow.AddDays(1);

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE));
        }

        [Fact]
        public void ErrorInvalidPaymentType()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Build();

            request.PaymentType = (PaymentType)5;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ErrorInvalidAmount(decimal amount)
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Build();

            request.Amount = amount;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void ErrorInvalidTitle(string title)
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Build();

            request.Title = title;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
        }

        [Fact]
        public void ErrorTagInvalid()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Build();

            request.Tags.Add((Tag)1000);

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TAG_TYPE_NOT_SUPPORTED));
        }
    }
}
