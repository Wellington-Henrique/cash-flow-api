using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseValidator : AbstractValidator<RequestRegisterExpenseJson>
    {
        public RegisterExpenseValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("The title is required");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("The amount must be greater then zero");
            RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Expenses cannot be for the future");
            RuleFor(x => x.PaymentType).IsInEnum().WithMessage("Payment type is not valid");
        }
    }
}
