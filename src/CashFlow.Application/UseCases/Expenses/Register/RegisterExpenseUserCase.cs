﻿using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUserCase
    {
        public ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request )
        {
            Validate(request);

            var entity = new Expense
            {
                Amount = request.Amount,
                Date = request.Date,
                Description = request.Description,
                PaymentType = (Domain.Enums.PaymentType)request.PaymentType,
                Title = request.Title,
            };

            return new ResponseRegisterExpenseJson();
        }

        private void Validate (RequestRegisterExpenseJson request)
        {
            var validator = new RegisterExpenseValidator();
            
            var result = validator.Validate(request);
        
            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
