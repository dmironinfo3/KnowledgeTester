using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.ExcelImporter;
using KT.Logger;

namespace KnowledgeTester.Code
{
	public class KtExcelHandler
	{
		private static IExcelParser _parser;

		public static IExcelParser Parser
		{
			get { return _parser; }
		}

		public static void Init()
		{
			_parser = new ExcelParser();
		}
	}
}