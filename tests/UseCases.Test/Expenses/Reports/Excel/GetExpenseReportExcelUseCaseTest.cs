﻿using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.Reports.Excel
{
    public class GetExpenseReportExcelUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(loggedUser);

            var useCase = CreateUseCase(loggedUser, expenses);

            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task SuccessEmpty()
        {
            var loggedUser = UserBuilder.Build();
            var expenses = new List<Expense>();

            var useCase = CreateUseCase(loggedUser, expenses);

            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

            result.Should().BeEmpty();
        }

        private GenerateExpensesReportExcelUseCase CreateUseCase(User user, List<Expense> expenses)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder()
                                    .FilterByMonth(user, expenses)
                                    .Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            return new GenerateExpensesReportExcelUseCase(repository, loggedUser);
        }
    }
}