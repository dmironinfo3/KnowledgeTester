using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class GeneratedQuestionDto : BaseDto
	{	
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public Guid GeneratedTestId { get; set; }

		[DataMember]
		public string Argument { get; set; }

		[DataMember]
		public QuestionDto Question { get; set; }

		[DataMember]
		public IEnumerable<GeneratedAnswerDto> SelectedAnswers { get; set; }
	}
}
