using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class UserDto: BaseDto
	{
		[DataMember]
		public string Username { get; set; }

		[DataMember]
		public string Password { get; set; }

		[DataMember]
		public string PasswordHint { get; set; }

		[DataMember]
		public string Email { get; set; }

		[DataMember]
		public string FirstName { get; set; }

		[DataMember]
		public string LastName { get; set; }

		[DataMember]
		public bool IsAdmin { get; set; }

		[DataMember] 
		public IEnumerable<CategoryDto> Categories { get; set; }

		[DataMember]
		public IEnumerable<TestDto> Subscriptions { get; set; }

		[DataMember] 
		public IEnumerable<GeneratedTestDto> MyTests { get; set; }
	}
}
