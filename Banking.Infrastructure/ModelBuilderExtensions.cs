using Microsoft.EntityFrameworkCore;
using Banking.Domain.Entities;
using Banking.Domain.Entities.Operations;

namespace Banking.Infrastructure
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Admin", Phone = "+55 51 987654321", Password = "admin", Email = "admin@gmail.com", Enabled = true },
                new User { Id = 2, Name = "Miguel", Phone = "+55 51 987654321", Password = "miguel", Email = "miguel@gmail.com", Enabled = true },
                new User { Id = 3, Name = "Diogo", Phone = "+55 51 987654321", Password = "diogo", Email = "diogo@gmail.com", Enabled = true },
                new User { Id = 4, Name = "Bruna", Phone = "+55 51 987654321", Password = "bruna", Email = "bruna@gmail.com", Enabled = true }
            );

            modelBuilder.Entity<Branch>().HasData(
                new Branch { Id = 1, Number = 1234 },
                new Branch { Id = 2, Number = 4567 }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, Code = "123456", UserId = 1, BranchId = 1, Enabled = true, Balance = 0, Rentabilize = false },
                new Account { Id = 2, Code = "122334", UserId = 2, BranchId = 1, Enabled = true, Balance = 10000m, Rentabilize = true },
                new Account { Id = 3, Code = "365654", UserId = 3, BranchId = 2, Enabled = true, Balance = 10000m, Rentabilize = true },
                new Account { Id = 4, Code = "567989", UserId = 4, BranchId = 2, Enabled = true, Balance = 10000m, Rentabilize = true }
            );
        }
    }
}
