namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
    public interface IGenerateExpensesRetportPdfUseCase
    {
        Task<byte[]> Execute(DateOnly month);
    }
}
