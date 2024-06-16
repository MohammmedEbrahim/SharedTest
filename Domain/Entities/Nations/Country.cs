using System.Collections.Generic;

namespace Domain.Entities.Nations
{
	public class Country
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int Order { get; set; }
		public bool IsActive { get; set; }
	}
}
