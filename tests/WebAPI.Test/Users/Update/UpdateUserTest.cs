using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebAPI.Test.InlineData;

namespace WebAPI.Test.Users.Update
{
    public class UpdateUserTest : CashFlowClassFixture
    {
        private const string METHOD = "api/User";

        private readonly string _token;

        public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var result = await DoPut(METHOD, request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorEmptyName(string culture)
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var result = await DoPut(requestUri: METHOD, request: request, token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement
                .GetProperty("errorMessages")
                .EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager
                .GetString("NAME_EMPTY", new CultureInfo(culture));

            errors
                .Should()
                .HaveCount(1)
                .And
                .Contain(error =>
                    error
                    .GetString()!
                    .Equals(expectedMessage)
                );
        }
    }
}
