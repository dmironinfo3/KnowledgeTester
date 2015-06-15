using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KT.DTOs.Objects
{
	[DataContract]
	public class TestReviewAnswerDto:BaseDto
	{
		[DataMember]
		public bool IsSelected { get; set; }
		[DataMember]
		public string Text { get; set; }
		[DataMember]
		public bool IsCorrect { get; set; }
	}
}
