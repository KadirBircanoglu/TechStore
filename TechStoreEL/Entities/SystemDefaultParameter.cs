using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.Entities
{
	public class SystemDefaultParameter:BaseNumeric<int>
	{
		public string Name { get; set; }
		public string? Description { get; set; }
		public string Value { get; set; }
		public bool IsDeleted { get; set; }
	}
}
