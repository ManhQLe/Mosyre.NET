using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mosyre;

namespace Test
{
	public class TestConduit : TestFrame, IClay
	{
		int Result = 0;
		IClay Clay;
		public void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{
		}

		public void onConnection(IClay withClay, object atConnectionPoint)
		{
			Clay = withClay;
		}

		object Signal {
			set {
				Clay.onCommunication(this, "X", value);
			}
		}

		public override void Test()
		{
			int Result = 0;
			int Result2 = 0;
			ResponseFunc f = (RClay rc, object cp) => {
				int A = Result = rc.GetSignals<int>("X");
				int B = Result = rc.GetSignals<int>("Y");
				Result = A + B;
			};

			ResponseFunc f2 = (RClay rc, object cp) =>
			{
				Result2 = rc.GetSignals<int>("A");
			};

			RClay R = new RClay(new Dictionary<string, object>(){
				{ "Response", f},
				{ "ConnectPoints",new List<Object>{ "X","Y" }}
			});

			RClay R2 = new RClay(new Dictionary<string, object> {
				{ "Response",f2},
				{ "ConnectPoints",new List<Object>{"A"} }
			});


			Conduit con = Conduit.Link(this, "X", "X", R);
			
			con.Connect(R, "Y");
			con.Connect(R2, "A");

			con.ParallelTrx = false; //Disable parallel transmission

			Signal = 1;

			Assert(Result, 2);
			Assert(Result2, 1);
			Signal = 2;

			Assert(Result, 4);
			Assert(Result2, 2);


			con.ParallelTrx = true; //Enable parallel transmission
			Signal = 3;
			Thread.Sleep(100); //Wait

			Assert(Result, 6);
			Assert(Result2, 3);

		}
	}
}
