using CashFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using Validators.Tests.Requests;
using WebAPI.Test.InlineData;

namespace WebAPI.Test.Expenses.Update
{
    public class UpdateExpenseTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";

        private readonly string _token;
        private readonly long _expenseId;

        public UpdateExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _expenseId = webApplicationFactory.Expense_MemberTeam.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestExpenseJsonBuilder.Build();

            var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorEmptyTitle(string culture)
        {
            var request = RequestExpenseJsonBuilder.Build();
            request.Title = string.Empty;

            var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement
                .GetProperty("errorMessages")
                .EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

            errors
                .Should()
                .HaveCount(1)
                .And
                .Contain(c => c.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ExpenseNotFound(string culture)
        {
            var request = RequestExpenseJsonBuilder.Build();

            var result = await DoPut(requestUri: $"{METHOD}/{1000}", request: request, token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement
                .GetProperty("errorMessages")
                .EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

            errors
                .Should()
                .HaveCount(1)
                .And
                .Contain(c => c.GetString()!.Equals(expectedMessage));
        }
    }
}
