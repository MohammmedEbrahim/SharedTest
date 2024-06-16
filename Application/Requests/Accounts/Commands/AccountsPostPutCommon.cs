using Application.Common.Interfaces;
using Domain.Enums;
using FluentValidation;
using System.Linq;

namespace Application.Requests.Accounts.Commands
{
	public class AccountsPostPutCommon
	{
		public string Email { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public string PhoneNumber { get; set; }
		public Role Role { get; set; }

		public class CommonValidator<T> : AbstractValidator<T> where T : AccountsPostPutCommon
		{
			private readonly IApplicationDbContext _context;

			public CommonValidator(IApplicationDbContext context)
			{
				_context = context;
				
				RuleFor(s => s.Email)
				.Matches(@"^([a-zA-Z0-9]+@+[a-z]+\.+[a-z])")
				.WithMessage("Invalid Email Format");

				RuleFor(a => a.UserName)
					.NotNull()
					.WithMessage("UserName can't be null");
				
				RuleFor(a => a.Password)
					.NotEmpty()
					.MinimumLength(8)
					.Matches("[A-Z]").WithMessage(" Password must contain one or more capital letters.")
					.Matches("[a-z]").WithMessage(" Password must contain one or more lowercase letters.")
					.Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage(" Password must contain one or more special characters.")
					.WithMessage(" Password contains that is not allowed.");

				RuleFor(a => a.ConfirmPassword)
					.Equal(a => a.Password)
					.WithMessage("Confirm Password must equal password");

				RuleFor(a => a.PhoneNumber).NotNull().Must((phone) =>
				{
					return ( !_context.Accounts.Any(a => a.PhoneNumber == phone) );
				}).WithMessage("Phone Is Exists");
				
				RuleFor(a => a.Role)
					.IsInEnum()
					.WithMessage("Should choose from exists");
			}
		}
	}
}
