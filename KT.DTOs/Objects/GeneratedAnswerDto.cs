using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class GeneratedAnswerDto:BaseDto
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public Guid GeneratedQuestionId { get; set; }

		[DataMember]
		public AnswerDto Answer { get; set; }

		[DataMember]
		public bool IsSelected { get; set; }
	}
}
