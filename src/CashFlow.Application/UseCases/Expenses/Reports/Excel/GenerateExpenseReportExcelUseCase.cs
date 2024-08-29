using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel
{
    internal class GenerateExpenseReportExcelUseCase : IGenerateExpenseReportExcelUseCase
    {
        public Task<byte[]> Execute(DateOnly month)
        {
            var workbook = new XLWorkbook();

            workbook.Author = "Wellington Henrique";
            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Times New Roman";

            var worksheet = workbook.Worksheets.Add(month.ToString("Y"));
            
        }
    }
}
