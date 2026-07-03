using Microsoft.EntityFrameworkCore;
using MiAlma.Domain.Entities;

namespace MiAlma.Infrastructure.Persistence
{
    public class MiAlmaDbContext : DbContext
    {
        public MiAlmaDbContext(DbContextOptions<MiAlmaDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Rfp> Rfps => Set<Rfp>();
        public DbSet<Proposal> Proposals => Set<Proposal>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiAlmaDbContext).Assembly);
        }
    }
}
