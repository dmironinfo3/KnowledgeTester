using System;
using System.Runtime.Serialization;

namespace KT.DTOs.Objects
{
[DataContract]
	public class TestReviewQuestionDto: BaseDto
	{
		[DataMember]
		public Guid Id { get; set; }
		[DataMember]
		public string Text { get; set; }
		[DataMember]
		public TestReviewAnswerDto[] Answers { get; set; }
		[DataMember]
		public string Argument { get; set; }
		[DataMember]
		public string CorrectArgument { get; set; }
	}
}