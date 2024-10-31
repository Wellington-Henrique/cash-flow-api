using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using System.Data;

namespace CashFlow.Application.UseCases.Expenses.Update
{
    public class UpdateExpenseUseCase : IUpdateExpenseUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExpensesUpdateOnlyRepository _expensesRepository;
        private readonly ILoggedUser _loggedUser;

        public UpdateExpenseUseCase(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IExpensesUpdateOnlyRepository expensesRepository,
            ILoggedUser loggedUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _expensesRepository = expensesRepository;
            _loggedUser = loggedUser;
        }

        public async Task Execute(long id, RequestExpenseJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.Get();

            var expense = await _expensesRepository.GetById(loggedUser, id);

            if (expense is null)
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

            _mapper.Map(request, expense);
            _expensesRepository.Update(expense);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestExpenseJson request)
        {
            var validator = new ExpenseValidator();

            var result = validator.Validate(request);

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
