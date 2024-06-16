using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services
{
	public class CurrentUserService : ICurrentUserService
	{
		private readonly IHttpContextAccessor _accessor;

		public CurrentUserService(IHttpContextAccessor accessor) =>  _accessor = accessor;
		
		public string Get(string claim) => _accessor.HttpContext.User.FindFirstValue(claim);
		
	}
}
