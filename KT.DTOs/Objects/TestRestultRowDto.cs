using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class TestRestultRowDto
	{
		[DataMember]
		public string Username { get; set; }
		[DataMember]
		public int Score { get; set; }
		[DataMember]
		public string FullName { get; set; }
		[DataMember]
		public bool Validated { get; set; }
	}
}
