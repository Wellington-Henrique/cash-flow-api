using FluentAssertions;
using System.Net;
using System.Net.Mime;
using Validators.Tests.Requests;

namespace WebAPI.Test.Expenses.Reports
{
    public class GenerateExpenseReportTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Report";

        private readonly string _adminToken;
        private readonly string _teamMemberToken;
        private readonly DateTime _expenseDate;

        public GenerateExpenseReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _adminToken = webApplicationFactory.User_Admin.GetToken();
            _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
            _expenseDate = webApplicationFactory.Expense_MemberAdmin.GetDate();
        }

        [Fact]
        public async Task SuccessPdf()
        {
            var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate:yyyy-MM}", token: _adminToken);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.Should().NotBeNull();
            result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
        }

        [Fact]
        public async Task SuccessExcel()
        {
            var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:yyyy-MM}", token: _adminToken);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.Should().NotBeNull();
            result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
        }

        [Fact]
        public async Task ErrorForbiddenUserNotAllowedPdf()
        {
            var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate:Y}", token: _teamMemberToken);

            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task ErrorForbiddenUserNotAllowedExcel()
        {
            var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:Y}", token: _teamMemberToken);

            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
