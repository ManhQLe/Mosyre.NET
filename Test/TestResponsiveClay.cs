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
			ResponsiveClay other = new ResponsiveClay();

			int Result = 0;
			int Inited = 0;
			ResponseFunc r = (ResponsiveClay rc, object cp) => {
				int A = rc.GetSignals<int>("A");
				int B = rc.GetSignals<int>("B");
				Result = A + B;
			};
			InitFunc i = (ResponsiveClay rc) => { Inited++; };

			ResponsiveClay c = new ResponsiveClay(new Dictionary<string, object> {
				{"P1",1 },
				{"P2",2 },
				{"ConnectPoints", new List<object>{ "A","B" } },
				{"Response",r },
				{"Init",i }
			});
			c.onConnection(this, "A");
			c.onConnection(this, "B");

			c.onCommunication(this, "A", 1);
			//Make sure that if clay has not collected every signal
			//There will be no response
			Assert(Result, 0);

			c.onCommunication(this, "B", 2);

			Assert(Result, 3);

			//Randomly check Init already got called and got called once only
			Assert(Inited, 1);

			c.onCommunication(this, "A", 2);

			Assert(Result, 4);

			//Make sure signal from stranger does not count
			c.onCommunication(other, "A", 6);
			Assert(Result, 4);
			
			c.Stage = true;

			c.onCommunication(this, "B", 4);
			Assert(Result, 6);

			//Randomly check Init already got called and got called once only
			Assert(Inited, 1);

			c.onCommunication(this, "B", 7);
			//Make sure that Stage did clear all signals
			//And result should be the same as the previous
			Assert(Result, 6);
			c.onCommunication(this, "A", 8);
			Assert(Result, 15);

			c.onCommunication(this, "B", 1);
			//Make sure that all signals got cleared again
			//And the result is still the same because response has not been called
			Assert(Result, 15);
		}
	}
}
