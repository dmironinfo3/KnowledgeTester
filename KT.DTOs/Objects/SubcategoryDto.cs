using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class SubcategoryDto: BaseDto
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public Guid CategoryId { get; set; }

		[DataMember]
		public IEnumerable<QuestionDto> Questions { get; set; }

		[DataMember]
		public IEnumerable<TestDto> Tests { get; set; }
	}
}
