using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class QuestionDto : BaseDto
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public string Text { get; set; }

		[DataMember]
		public string Argument { get; set; }

		[DataMember]
		public bool MultipleResponse { get; set; }

		[DataMember]
		public Guid SubcategoryId { get; set; }

		[DataMember]
		public IEnumerable<AnswerDto> Answers { get; set; }
	}
}
