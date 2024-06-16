using Domain.Entities.Nations;

namespace Application.Requests.Countries.Models
{
	public class CountryVm
	{
		public string Name { get; set; }
		public string Code { get; set; }

		public static CountryVm MapFrom(Country source)
			=> new()
			{
				Name = source.Name ,
				Code = source.Code
			};
	}
}
