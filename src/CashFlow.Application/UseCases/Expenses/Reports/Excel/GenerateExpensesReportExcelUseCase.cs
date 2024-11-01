using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel
{
    public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
    {
        private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
        private const string CURRENCY_SYMBOL = "€";
        private readonly ILoggedUser _loggedUser;

        public GenerateExpensesReportExcelUseCase(
            IExpensesReadOnlyRepository expensesReadOnlyRepository,
            ILoggedUser loggedUser
            )
        {
            _expensesReadOnlyRepository = expensesReadOnlyRepository;
            _loggedUser = loggedUser;
        }

        public async Task<byte[]> Execute(DateOnly month)
        {
            var loggedUser = await _loggedUser.Get();

            var expenses = await _expensesReadOnlyRepository.FilterByMonth(loggedUser, month);

            if (expenses.Count == 0) return [];

            // O using garante que não é necessário usar o workbook.Dispose() ao final deste bloco
            // liberando recursos do sistema operacional
            using var workbook = new XLWorkbook();

            workbook.Author = loggedUser.Name;
            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Times New Roman";

            var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

            InsertHeader(worksheet);

            int line = 1;

            foreach(var expense in expenses)
            {
                line++;
                worksheet.Cell($"A{line}").Value = expense.Date;
                worksheet.Cell($"B{line}").Value = expense.Title;
                worksheet.Cell($"C{line}").Value = expense.Description;
                worksheet.Cell($"D{line}").Value = expense.PaymentType.PaymentTypeToString();
                worksheet.Cell($"E{line}").Value = expense.Amount;
                worksheet.Cell($"E{line}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";
            }

            var file = new MemoryStream();

            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(file);
            //workbook.Dispose();
            return file.ToArray();
        }

        private void InsertHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = ResourceReportGenerationMessages.DATE;
            worksheet.Cell("B1").Value = ResourceReportGenerationMessages.TITLE;
            worksheet.Cell("C1").Value = ResourceReportGenerationMessages.DESCRIPTION;
            worksheet.Cell("D1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
            worksheet.Cell("E1").Value = ResourceReportGenerationMessages.AMOUNT;

            worksheet.Cells("A1:E1").Style.Font.Bold = true;
            worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");
            
            worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        }
    }
}
