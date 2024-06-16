using Domain.Entities.Accounts;
using Domain.Entities.Nations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
	{
        public DbSet<Account> Accounts { get; set; }
		public DbSet<Country> Countries { get; set; }

		public Task<int> SaveChangesAsync();
		public int SaveChanges();
	}
}
