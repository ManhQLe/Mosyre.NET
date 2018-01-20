using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mosyre;
namespace Test
{
	public class TestResponsiveClay : TestFrame, IClay
	{
		public void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{
			
		}

		public void onConnection(IClay withClay, object atConnectionPoint)
		{
			
		}

		public override void Test()
		{
			int Result = 0;
			ResponseFunc r = (ResponsiveClay rc, object cp) => {
				int A = rc.GetSignals<int>("A");
				int B = rc.GetSignals<int>("B");
				Result = A + B;
			};
			InitFunc i = (ResponsiveClay rc) => { };

			ResponsiveClay c = new ResponsiveClay(new Dictionary<string, object> {
				{"P1",1 },
				{"P2",2 },
				{"ConnectPoints", new List<object>{ "A","B" } },
				{"Response",r }
			});
			c.onConnection(this, "A");
			c.onConnection(this, "B");

			c.onCommunication(this, "A", 1);
			c.onCommunication(this, "B", 2);

			Assert(Result, 3);

			c.onCommunication(this, "A", 2);

			Assert(Result, 4);

		}
	}
}
