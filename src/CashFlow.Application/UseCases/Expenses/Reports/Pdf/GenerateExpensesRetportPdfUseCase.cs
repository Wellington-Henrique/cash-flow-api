
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
    internal class GenerateExpensesRetportPdfUseCase : IGenerateExpensesRetportPdfUseCase
    {
        private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
        private const string CURRENCY_SYMBOL = "€";

        public GenerateExpensesRetportPdfUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository)
        {
            _expensesReadOnlyRepository = expensesReadOnlyRepository;
        }

        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await _expensesReadOnlyRepository.FilterByMonth(month);

            if (expenses.Count == 0) return [];

            return [];
        }
    }
}
