using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class TestDto: BaseDto
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public DateTime StartTime { get; set; }

		[DataMember]
		public DateTime EndTime { get; set; }

		[DataMember]
		public int Duration { get; set; }

		[DataMember]
		public Guid SubcategoryId { get; set; }

		[DataMember]
		public int Questions { get; set; }
	}
}
