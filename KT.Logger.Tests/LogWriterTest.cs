using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using KT.Logger.ObserverPattern;
using NUnit.Framework;

namespace KT.Logger.Tests
{
	[TestFixture]
	public class LogWriterTest
	{
		private const int Capacity = 3;

		[TestFixtureSetUp]
		public void SetUp()
		{
			LogHandler.Init("DEBUG", Capacity);

			ClearFiles();
		}

		[TearDown]
		public void TearDown()
		{
			ClearFiles();
		}

		[Test]
		public void WhenWritingALogEntry()
		{
			WriteEnt(1);

			var files = Directory.GetFiles(Environment.CurrentDirectory, "ktLog*.txt");

			Assert.AreEqual(files.Length, 0);
		}

		[Test]
		public void WhenNeedingToSwitchFile()
		{
			//creating new 10 MB file
			File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "ktLog_test.txt"), new byte[1048576 + 1]);

			//needed to force the file switch
			WriteEnt(11);

			Assert.That(Completed(2), Is.True);
		}

		[Test]
		public void WhenWritingManyLogEntries()
		{
			WriteEnt(11);

			var completed = Completed(1);

			var files = Directory.GetFiles(Environment.CurrentDirectory, "ktLog*.txt");

			var file = files.First();

			Assert.That(completed, Is.True);
			Assert.IsNotNull(file);
			Assert.That(File.ReadAllLines(file).Length, Is.GreaterThanOrEqualTo(10));
		}

		private static void WriteEnt(int count)
		{
			for (int i = 1; i <= count; i++)
			{
				(new Logger()).Error("message", "username", new DivideByZeroException("TEST EXCEPTION"));
			}
		}

		private static void ClearFiles()
		{
			var files = Directory.GetFiles(Environment.CurrentDirectory, "ktLog*.txt");

			foreach (var file in files)
			{
				File.Delete(file);
			}
		}

		private static bool Completed(int filesCount)
		{
			//waiting for the thread to complete

			var files = Directory.GetFiles(Environment.CurrentDirectory, "ktLog*.txt");
			
			var completed = false;
			var iterations = 0;
			var maxIterations = 60;
			while (!completed && iterations <= maxIterations)
			{
				Thread.Sleep(1000);

				if (files.Length >= filesCount)
				{
					completed = true;
					continue;
				}

				files = Directory.GetFiles(Environment.CurrentDirectory, "ktLog*.txt");
				iterations++;
			}
			return completed;
		}
	}
}
