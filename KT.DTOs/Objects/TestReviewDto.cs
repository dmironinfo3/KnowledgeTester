using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class TestReviewDto:BaseDto
	{
		[DataMember]
		public Guid Id { get; set; }
		[DataMember]
		public Guid TestId { get; set; }
		[DataMember]
		public string Username { get; set; }
		[DataMember]
		public TestReviewQuestionDto[] Questions { get; set; }
		[DataMember]
		public int Score { get; set; }
	}
}
