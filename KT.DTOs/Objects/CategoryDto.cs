using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class CategoryDto: BaseDto
	{

		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public UserDto CreatedBy { get; set; }

		[DataMember]
		public string Name { get; set; }
	}
}
