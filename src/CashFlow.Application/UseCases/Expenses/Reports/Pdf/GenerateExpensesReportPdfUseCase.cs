
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
    public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
    {
        private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
        private const string CURRENCY_SYMBOL = "€";
        private const int HEIGHT_ROW_EXPENSE_TABLE = 25;
        private readonly ILoggedUser _loggedUser;

        public GenerateExpensesReportPdfUseCase(
            IExpensesReadOnlyRepository expensesReadOnlyRepository,
            ILoggedUser loggedUser
            )
        {
            _expensesReadOnlyRepository = expensesReadOnlyRepository;
            _loggedUser = loggedUser;

            GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
        }

        public async Task<byte[]> Execute(DateOnly month)
        {
            var loggedUser = await _loggedUser.Get();

            var expenses = await _expensesReadOnlyRepository.FilterByMonth(loggedUser, month);

            if (expenses.Count == 0) return [];

            var document = CreateDocument(loggedUser, month);
            var page = CreatePage(document);

            CreateHeaderWithProfilePhotoAndName(loggedUser, page);

            var totalExpenses = expenses.Sum(expense => expense.Amount);

            CreateTotalSpentSection(page, month, totalExpenses);

            foreach( var expense in expenses)
            {
                var table = CreateExpenseTable(page);

                var row = table.AddRow();
                row.Height = HEIGHT_ROW_EXPENSE_TABLE;

                AddExpenseTitle(row.Cells[0], expense.Title);
                AddHeaderForAmount(row.Cells[3]);

                row = table.AddRow();
                row.Height = HEIGHT_ROW_EXPENSE_TABLE;

                row.Cells[0].AddParagraph(expense.Date.ToString("D"));
                row.Cells[0].Format.LeftIndent = 20;
                SetStyleBaseForExpenseInformation(row.Cells[0]);

                row.Cells[1].AddParagraph(expense.Date.ToString("t"));
                SetStyleBaseForExpenseInformation(row.Cells[1]);
                               
                row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
                SetStyleBaseForExpenseInformation(row.Cells[2]);

                AddAmountForExpense(row.Cells[3], expense.Amount);

                if (string.IsNullOrWhiteSpace(expense.Description) == false)
                {
                    var description = table.AddRow();
                    description.Height = HEIGHT_ROW_EXPENSE_TABLE;
                    description.Cells[0].AddParagraph(expense.Description);
                    description.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
                    description.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                    description.Cells[0].MergeRight = 2;
                    description.Cells[0].Format.LeftIndent = 20;
                   
                    description.Cells[0].Format.Font = new Font
                    {
                        Name = FontHelper.WORKSANS_REGULAR,
                        Size = 10,
                        Color = ColorsHelper.BLACK
                    };

                    row.Cells[3].MergeDown = 1;
                }

                AddWhiteSpace(table);
            }

            return RenderDocument(document);
        }

        private Document CreateDocument(Domain.Entities.User user, DateOnly month)
        {
            var document = new Document();

            document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}";
            document.Info.Author = user.Name;

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

        private void CreateHeaderWithProfilePhotoAndName(Domain.Entities.User user, Section page)
        {
            var table = page.AddTable();
            table.AddColumn();
            table.AddColumn("300");

            var row = table.AddRow();

            var assembly = Assembly.GetExecutingAssembly();
            var directoryName = Path.GetDirectoryName(assembly.Location);

            // Nas proriedades da imagem habilitar a opção Copy always (Copiar sempre)
            var pathFile = Path.Combine(directoryName, "Logo", "logo.png");

            row.Cells[0].AddImage(pathFile);
            row.Cells[1].AddParagraph($"Olá, {user.Name}!");
            row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
            row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        }
               
        private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
        {
            var paragraph = page.AddParagraph();

            paragraph.Format.SpaceBefore = "40";
            paragraph.Format.SpaceAfter = "40";

            var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

            paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });
            paragraph.AddLineBreak();


            paragraph.AddFormattedText($"{totalExpenses:f2} {CURRENCY_SYMBOL}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });

        }

        private Table CreateExpenseTable(Section page)
        {
            var table = page.AddTable();

            table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

            return table;
        }

        private void AddExpenseTitle(Cell cell, string title)
        {
            cell.AddParagraph(title);
            cell.Shading.Color = ColorsHelper.RED_LIGHT;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.MergeRight = 2;
            cell.Format.LeftIndent = 20;

            cell.Format.Font = new Font
            {
                Name = FontHelper.RALEWAY_BLACK,
                Size = 14,
                Color = ColorsHelper.BLACK
            };
        }
        
        private void AddHeaderForAmount(Cell cell)
        {
            cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
            cell.Shading.Color = ColorsHelper.RED_DARK;
            cell.VerticalAlignment = VerticalAlignment.Center;

            cell.Format.Font = new Font
            {
                Name = FontHelper.RALEWAY_BLACK,
                Size = 14,
                Color = ColorsHelper.WHITE
            };
        }
        
        private void SetStyleBaseForExpenseInformation(Cell cell)
        {
            cell.Shading.Color = ColorsHelper.GREEN_DARK;
            cell.VerticalAlignment = VerticalAlignment.Center;

            cell.Format.Font = new Font
            {
                Name = FontHelper.WORKSANS_REGULAR,
                Size = 12,
                Color = ColorsHelper.BLACK
            };
        }

        private void AddAmountForExpense(Cell cell, decimal amount)
        {

            cell.AddParagraph($"-{amount:f2} {CURRENCY_SYMBOL}");
            cell.Shading.Color = ColorsHelper.WHITE;
            cell.VerticalAlignment = VerticalAlignment.Center;

            cell.Format.Font = new Font
            {
                Name = FontHelper.WORKSANS_REGULAR,
                Size = 14,
                Color = ColorsHelper.BLACK
            };
        }        
        
        private void AddWhiteSpace(Table table)
        {
            var row = table.AddRow();
            row.Height = 30;
            row.Borders.Visible = false;
        }

        private byte[] RenderDocument(Document document)
        {
            var renderer = new PdfDocumentRenderer
            {
                Document = document,
            };

            renderer.RenderDocument();

            using var file = new MemoryStream();

            renderer.PdfDocument.Save(file);

            return file.ToArray();
        }
    }
}
