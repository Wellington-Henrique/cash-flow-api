using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories.Expenses;
internal class ExpensesRepository : 
    IExpensesWriteOnlyRepository, 
    IExpensesReadOnlyRepository, 
    IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;
    
    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task Delete(long id)
    {
        var result = await _dbContext.Expenses.FindAsync(id);

        _dbContext.Expenses.Remove(result!);
    }

    public async Task<List<Expense>> GetAll(Domain.Entities.User user)
    {
        return await _dbContext.Expenses
                                .AsNoTracking()
                                .Where(expense => expense.UserId == user.Id)
                                .ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(Domain.Entities.User user, long id)
    {
        return await _dbContext.Expenses
                                .AsNoTracking()
                                .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }    
    
    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(Domain.Entities.User user, long id)
    {
        return await _dbContext.Expenses
                                .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    public void Update(Expense entity)
    {
        _dbContext.Expenses.Update(entity);
    }

    public async Task<List<Expense>> FilterByMonth(Domain.Entities.User user, DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
        var day = DateTime.DaysInMonth(startDate.Year, startDate.Month);

        var endDate = new DateTime(year: date.Year, month: date.Month, day: day, hour: 23, minute: 59, second: 59);

        return await _dbContext.Expenses
                                .AsNoTracking()
                                .Where(expense => user.Id == expense.UserId && expense.Date >= startDate && expense.Date <= endDate)
                                .OrderByDescending(expense => expense.Date)
                                .ThenByDescending(expense => expense.Title)
                                .ToListAsync();
    }
}