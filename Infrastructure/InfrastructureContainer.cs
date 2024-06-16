using Application.Common.Interfaces;
using Domain.Entities.Accounts;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureContainer
	{
		public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(option =>
				option.UseSqlServer(configuration.GetConnectionString("DBConnect")));

			services.AddIdentityCore<Account>()
				.AddSignInManager()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			// Register the service and implementation for the database context
			services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

			services.Configure<IdentityOptions>(options =>
			{
				// Default Password settings.
				options.Password.RequireDigit           = false;
				options.Password.RequireLowercase       = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase       = false;
				options.Password.RequiredLength         = 6;
				//options.Password.RequiredUniqueChars    = 2;
			});

			
			return services;
		}
	}
}

