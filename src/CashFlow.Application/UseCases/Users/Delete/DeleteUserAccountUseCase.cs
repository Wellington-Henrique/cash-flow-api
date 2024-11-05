﻿using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Delete
{
    public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserWriteOnlyRepository _repository;

        public DeleteUserAccountUseCase(
            ILoggedUser loggedUser, 
            IUnitOfWork unitOfWork,
            IUserWriteOnlyRepository repository)
        {
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task Execute()
        {
            var user = await _loggedUser.Get();

            await _repository.Delete(user);

            await _unitOfWork.Commit();
        }
    }
}
