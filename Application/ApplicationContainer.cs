using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Application
{
    public static class ApplicationContainer
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services )
		{
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			services.AddTransient(typeof(IPipelineBehavior<,>) , typeof(ValidationBehavior<,>));

			services.AddMediatR(Assembly.GetExecutingAssembly());
			
			services.TryAddSingleton<IHttpContextAccessor , HttpContextAccessor>();
			services.AddHttpContextAccessor();

			services.AddScoped<IAuthService , AuthService>();
			
			services.AddScoped<ICurrentUserService , CurrentUserService>();

			return services;
		}
	}
}

