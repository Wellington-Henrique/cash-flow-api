using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess
{
    internal class CashFlowDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=localhost;Database=cashflowdb;Uid=root;pwd=@Password123;";
            var version = new Version(8, 0, 35);
            var serverVerison = new MySqlServerVersion(version);

            optionsBuilder.UseMySql(connectionString, serverVerison);
        }
    }
}
