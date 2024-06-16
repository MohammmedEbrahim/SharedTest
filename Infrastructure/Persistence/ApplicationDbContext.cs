using Application.Common.Interfaces;
using Domain.Entities.Accounts;
using Domain.Entities.Nations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<Account>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);
			
		}

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Country> Countries { get; set; }
		
        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

		public int SaveChanges() => base.SaveChanges();
	}
}
