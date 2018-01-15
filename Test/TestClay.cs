using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mosyre;

namespace Test
{
	
	public class TestClay: TestFrame
	{
		public override void Test()
		{
			AttribClay C = new AttribClay(new Dictionary<string, object> {
				{"A",1 },
				{"B",false }
			});

			Assert(C.GetAgrementProp<int>("A"), 1);

			Assert(C.GetAgrementProp<int>("A", 2), 1);

			Assert(C.GetAgrementProp<bool>("B"), false);

			Assert(C.GetAgrementProp<decimal>("C", 1.0m), 1.0m);

		}
	}
}
