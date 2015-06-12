using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class GeneratedTestDto: BaseDto
	{

		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public TestDto Test { get; set; }

		[DataMember]
		public UserDto User { get; set; }

		[DataMember]
		public IEnumerable<GeneratedQuestionDto> GeneratedQuestions { get; set; }

		[DataMember]
		public bool IsFinished { get; set; }

		[DataMember]
		public bool IsValidated { get; set; }

		[DataMember]
		public int MaxScore { get; set; }

		[DataMember]
		public int Score { get; set; }
	}
}
