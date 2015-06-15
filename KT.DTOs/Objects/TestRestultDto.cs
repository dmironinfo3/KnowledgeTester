using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class TestRestultDto
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int Subscriptions { get; set; }

		[DataMember]
		public double AverageScore { get; set; }
	}
}
