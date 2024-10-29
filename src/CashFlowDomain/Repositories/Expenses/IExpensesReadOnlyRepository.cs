using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesReadOnlyRepository
    {
        Task<List<Expense>> GetAll(Domain.Entities.User user);
        Task<Expense?> GetById(Domain.Entities.User user, long id);
        Task<List<Expense>> FilterByMonth(Domain.Entities.User user, DateOnly date);
    }
}
