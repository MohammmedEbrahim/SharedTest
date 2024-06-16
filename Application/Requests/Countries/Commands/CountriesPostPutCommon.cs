using FluentValidation;

namespace Application.Requests.Countries.Commands
{
	public class CountriesPostPutCommon
	{
		public string Name { get; set; }
		public string Code { get; set; }

		public class CommonValidator<T> : AbstractValidator<T> where T : CountriesPostPutCommon
		{
			public CommonValidator()
			{
				RuleFor(c => c.Name).NotNull();
				RuleFor(c => c.Code).NotNull();
			}
		}
	}
}
