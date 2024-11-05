using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess
{

    public class CashFlowDbContext : DbContext
    {
        public CashFlowDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Impede o acesso direto a tabela Tags, mas garante o uso da FK
            modelBuilder.Entity<Tag>().ToTable("Tags");
        }
    }
}
