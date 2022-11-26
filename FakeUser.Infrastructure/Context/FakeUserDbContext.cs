using Microsoft.EntityFrameworkCore;
using FakeUser.Domain.Entities;

namespace FakeUser.Infrastructure.Context
{
    public class FakeUserDbContext : DbContext
    {
        public FakeUserDbContext(DbContextOptions options) : base(options) 
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public DbSet<FakeUserEntity> FakeUsers { get; set; }
    }
}
