﻿using CashFlow.Exception;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Validators.Tests.Requests;

namespace WebAPI.Test.Users.Register
{
    public  class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "api/User";

        private readonly HttpClient _httpClient;

        public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

            result.StatusCode.Should().Be(HttpStatusCode.Created);
            
            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response.RootElement
                .GetProperty("name")
                .GetString()
                .Should()
                .Be(request.Name);

            response.RootElement
                .GetProperty("token")
                .GetString()
                .Should()
                .NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task ErrorEmptyName()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement
                .GetProperty("errorMessages")
                .EnumerateArray();

            errors
                .Should()
                .HaveCount(1)
                .And
                .Contain(error => 
                    error
                    .GetString()!
                    .Equals(ResourceErrorMessages.NAME_EMPTY)
                );
        }
    }
}
