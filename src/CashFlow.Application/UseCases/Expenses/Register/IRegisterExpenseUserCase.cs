using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public interface IRegisterExpenseUserCase
    {
        public Task<ResponseRegisterExpenseJson> Execute(RequestRegisterExpenseJson request);
    }
}
