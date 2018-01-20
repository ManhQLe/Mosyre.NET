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

			Assert(C.GetAgreementProp<int>("A"), 1);

			Assert(C.GetAgreementProp<int>("A", 2), 1);

			Assert(C.GetAgreementProp<bool>("B"), false);

			Assert(C.GetAgreementProp<decimal>("C", 1.0m), 1.0m);

		}
	}
}
