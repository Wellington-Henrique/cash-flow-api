﻿using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories.Expenses;
using CashFlow.Infrastructure.DataAccess.Repositories.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure
{
    public static class DepenenceInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);

            services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesUpdateOnlyRepository, ExpensesRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");

            var version = new Version(8, 0, 35);
            var serverVerison = new MySqlServerVersion(version);

            services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVerison));
        }
    }
}
