using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUserCase
    {
        public ResponseRegisterExpenseJson Execute(ResquestRegisterExpenseJson request )
        {
            return new ResponseRegisterExpenseJson();
        }
    }
}
