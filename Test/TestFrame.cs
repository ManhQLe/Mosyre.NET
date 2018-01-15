using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
	public abstract class TestFrame
	{
		public void Run(string[] args) {
			Console.ForegroundColor = ConsoleColor.Gray;

			Console.WriteLine($"------ Testing {GetType().FullName} ------");
			try
			{
				Test();
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Test passed");
			}
			catch (Exception ex) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex);
			}

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine($"------ End of {GetType().FullName} ------");
		}
		public abstract void Test();

		public void Assert(object actual, object expect) {
			if (!actual.Equals(expect))
				throw new Exception($"Actual: {actual} is not the same as {expect}");
		}
	}
}
