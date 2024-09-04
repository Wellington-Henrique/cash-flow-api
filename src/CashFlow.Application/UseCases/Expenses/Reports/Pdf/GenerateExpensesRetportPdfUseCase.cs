﻿
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

            var document = CreateDocument(month);
            var page = CreatePage(document);

            var paragraph = page.AddParagraph();
            var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));
            
            paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });
            paragraph.AddLineBreak();

            var totalExpenses = expenses.Sum(expense => expense.Amount);

            paragraph.AddFormattedText($"{totalExpenses} {CURRENCY_SYMBOL}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });

            return [];
        }

        private Document CreateDocument(DateOnly month)
        {
            var document = new Document();

            document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}";
            document.Info.Author = "Wellington Henrique";

            var style = document.Styles["Normal"];

            style!.Font.Name = FontHelper.RALEWAY_REGULAR;

            return document;
        }

        private Section CreatePage(Document document) {
            var section =  document.AddSection();

            section.PageSetup = document.DefaultPageSetup.Clone();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.LeftMargin = 40;
            section.PageSetup.RightMargin = 40;
            section.PageSetup.TopMargin = 80;
            section.PageSetup.BottomMargin = 80;

            return section;
        }
    }
}
