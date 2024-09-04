
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
    internal class GenerateExpensesRetportPdfUseCase : IGenerateExpensesRetportPdfUseCase
    {
        private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
        private const string CURRENCY_SYMBOL = "€";

        public GenerateExpensesRetportPdfUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository)
        {
            _expensesReadOnlyRepository = expensesReadOnlyRepository;

            GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
        }

        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await _expensesReadOnlyRepository.FilterByMonth(month);

            if (expenses.Count == 0) return [];

            return [];
        }

        private Document CreateDocument(DateOnly month)
        {
            var document = new Document();

            document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}";
            document.Info.Author = "Wellington Henrique";

            var style = document.Styles["Normal"];

            style!.Font.Name = FontHelper.RALEWAY_REGULAR;
        }
    }
}
