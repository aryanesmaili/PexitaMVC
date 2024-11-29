using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PexitaMVC.Core.Entites;

namespace PexitaMVC.Infrastructure.Data
{
    public class AppDBContext : IdentityDbContext<UserModel>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PayingSessionModel>()
                .HasMany(x => x.Payments)
                .WithOne(x => x.Session)
                .HasForeignKey(x => x.SessionID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserModel>()
                .HasMany(x => x.PayingSessions)
                .WithMany(x => x.Users)
                .UsingEntity(j => j.ToTable("UserSessions"));

            base.OnModelCreating(modelBuilder);
        }
        public required DbSet<PayingSessionModel> PaymentSessions { get; set; }
        public required DbSet<PaymentModel> Payments { get; set; }
    }
}
