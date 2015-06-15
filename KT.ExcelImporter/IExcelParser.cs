using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using KT.DTOs.Objects;

namespace KT.ExcelImporter
{
	public interface IExcelParser
	{
		IEnumerable<QuestionDto> Parse(StreamReader str);
	}
}
