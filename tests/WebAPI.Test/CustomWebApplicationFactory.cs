﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private User _user;
        private string _password;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var provider = services
                                    .AddEntityFrameworkInMemoryDatabase()
                                    .BuildServiceProvider();
                    
                    services.AddDbContext<CashFlowDbContext>(config =>
                    {
                        config.UseInMemoryDatabase("InMemoryDbForTesting");
                        config.UseInternalServiceProvider(provider);
                    });

                    var scope = services
                                    .BuildServiceProvider()
                                    .CreateAsyncScope();
                    
                    var dbContext = scope
                                        .ServiceProvider
                                        .GetRequiredService<CashFlowDbContext>(); 

                    var passwordEncripter = scope
                                                .ServiceProvider
                                                .GetRequiredService<IPasswordEncripter>();

                    StartDataBase(dbContext, passwordEncripter);
                });
        }

        private void StartDataBase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
        {
            _user = UserBuilder.Build();
            _password = _user.Password;
            _user.Password = passwordEncripter.Encrypt(_user.Password);

            dbContext.Users.Add(_user);

            dbContext.SaveChanges();
        }

        public string GetName() => _user.Name;
        public string GetEmail() => _user.Email;
        public string GetPassword() => _password;
    }
}
