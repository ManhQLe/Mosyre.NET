﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			TestClay t1 = new TestClay();
			t1.Run(args);

			TestResponsiveClay t2 = new TestResponsiveClay();
			t2.Run(args);

			TestConduit t3 = new TestConduit();
			t3.Run(args);

			var t4 = new TestSClay();
			t4.Run(args);
		}
	}
}
