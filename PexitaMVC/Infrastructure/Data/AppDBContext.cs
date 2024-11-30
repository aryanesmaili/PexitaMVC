using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PexitaMVC.Core.Entites;

namespace PexitaMVC.Infrastructure.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : IdentityDbContext<UserModel>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BillModel>()
                .HasMany(x => x.BillPayments)
                .WithOne(x => x.Bill)
                .HasForeignKey(x => x.BillID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserModel>()
                .HasMany(x => x.Bills)
                .WithMany(x => x.Users)
                .UsingEntity(j => j.ToTable("UserSessions"));

            modelBuilder.Entity<UserModel>()
                .HasMany(x => x.UserPayments)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            base.OnModelCreating(modelBuilder);
        }

        public required DbSet<BillModel> Bills { get; set; }
        public required DbSet<PaymentModel> Payments { get; set; }
    }
}
