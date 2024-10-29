using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebAPI.Test.InlineData;

namespace WebAPI.Test.Login.DoLogin
{
    public class DoLoginTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Login";

        private readonly string _name;
        private readonly string _email;
        private readonly string _password;

        public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _name = webApplicationFactory.GetName();
            _email = webApplicationFactory.GetEmail();
            _password = webApplicationFactory.GetPassword();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson {
                Email = _email,
                Password = _password
            };

            var result = await DoPost(requestUri: METHOD, request: request);

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(responseBody);

            response.RootElement
                .GetProperty("name")
                .GetString()
                .Should()
                .Be(_name);

            response.RootElement
                .GetProperty("token")
                .GetString()
                .Should()
                .NotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorInvalidLogin(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();

            var result = await DoPost(requestUri: METHOD, request: request, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var responseBody = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(responseBody);

            var errors = response.RootElement
                .GetProperty("errorMessages")
                .EnumerateArray();
            
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));
            
            errors
                .Should()
                .HaveCount(1)
                .And
                .Contain(c => c.GetString()!.Equals(expectedMessage));
        }
    }
}
