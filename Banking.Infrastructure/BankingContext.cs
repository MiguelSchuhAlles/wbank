using Microsoft.EntityFrameworkCore;
using Banking.Domain.Entities;
using Banking.Domain.Entities.Operations;

namespace Banking.Infrastructure
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.BranchId);

            modelBuilder.Entity<Operation>()
                .HasIndex(o => new { o.Date, o.AccountId });

            modelBuilder.Seed();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<TicketPayment> TicketPayments { get; set; }
    }
}
