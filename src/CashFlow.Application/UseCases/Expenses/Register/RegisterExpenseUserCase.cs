﻿using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUserCase : IRegisterExpenseUserCase
    {
        private readonly IExpensesRepository _expensesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterExpenseUserCase(
            IExpensesRepository expensesRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _expensesRepository = expensesRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseRegisterExpenseJson> Execute(RequestRegisterExpenseJson request)
        {
            Validate(request);

            var entity = _mapper.Map<Expense>(request);

            await _expensesRepository.Add(entity);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisterExpenseJson>(entity);
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
