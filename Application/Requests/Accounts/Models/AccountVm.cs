using Domain.Entities.Accounts;

namespace Application.Requests.Accounts.Models
{
	public class AccountVm
	{
		public string UserName { get; set; }
		public string Email { get; set; }

		public static AccountVm MapFrom(Account source)
			=> new()
			{
			};
	}
}
