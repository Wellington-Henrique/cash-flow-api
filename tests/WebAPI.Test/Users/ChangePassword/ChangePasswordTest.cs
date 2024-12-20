﻿using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebAPI.Test.InlineData;

namespace WebAPI.Test.Users.ChangePassword
{
    public class ChangePasswordTest : CashFlowClassFixture
    {
        private const string METHOD = "api/User/change-password";

        private readonly string _token;
        private readonly string _email;
        private readonly string _password;

        public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _email = webApplicationFactory.User_Team_Member.GetEmail();
            _password = webApplicationFactory.User_Team_Member.GetPassword();

        }

        [Fact]
        public async Task Success()
        {
            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = _password;
            
            var result = await DoPut(METHOD, request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var loginRequest = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            var response = await DoPost("api/Login", loginRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            loginRequest.Password = request.NewPassword;

            response = await DoPost("api/Login", loginRequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorPasswordDifferentCurrentPassword(string culture)
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            var result = await DoPut(METHOD, request, token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement
                                    .GetProperty("errorMessages")
                                    .EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager
                .GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));

            errors
                .Should()
                .HaveCount(1).And
                .Contain(c => c.GetString()!.Equals(expectedMessage));
        }
    }
}
