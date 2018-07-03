using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mosyre;
using System.Threading;

namespace Test
{
	class TestSClay: TestFrame, IClay
	{
		public void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{
			
		}

		public void onConnection(IClay withClay, object atConnectionPoint)
		{
			
		}

		public override void Test()
		{
			int SumR = 0;
			int MulR = 0;
			ResponseFunc f = (RClay rc, object cp) => {
				int A = rc.GetSignals<int>("X");
				int B = rc.GetSignals<int>("Y");				
				rc["Z"] = SumR = A + B;
			};

			ResponseFunc f2 = (RClay rc, object cp) => {
				int A = rc.GetSignals<int>("X");
				int B = rc.GetSignals<int>("Y");
				rc["Z"] = MulR = A * B;

			};

			RClay Add = new RClay(new Dictionary<string, object>(){
				{ "Response", f},
				{ "ConnectPoints",new List<Object>{ "X","Y" }}
			});

			RClay Mul = new RClay(new Dictionary<string, object>(){
				{ "Response", f2},
				{ "ConnectPoints",new List<Object>{ "X","Y" }}
			});

			Conduit.CreateLink(Add, "Z", Mul, "X");

			SClay s = new SClay(new Dictionary<string, object>
			{
				{
					"layoutMap", new List<SClayLayout> {
						new SClayLayout{
							HostConnectPoint = "A",
							AtConnectionPoint= "X",							
						    WithClay = Add
						},
						new SClayLayout{
							HostConnectPoint = "B",
							AtConnectionPoint = "Y",
							WithClay = Add
						},						
						new SClayLayout{
							HostConnectPoint = "C",
							AtConnectionPoint = "Y",
							WithClay = Mul
						}
					}
				}
			});

			Clay.MakeConnection(s, this, "A");
			Clay.MakeConnection(s, this, "B");
			Clay.MakeConnection(s, this, "C");
			

			s.onCommunication(this, "A", 2);

			Assert(SumR, 0);
			s.onCommunication(this, "B", 3);
			Assert(SumR, 5);

			s.onCommunication(this, "C", 8);
			Thread.Sleep(100);
			Assert(MulR, 40);

			s.onCommunication(this, "C", 4);
			Thread.Sleep(100);
			Assert(MulR, 20);


			s.onCommunication(this, "A", 3);
			Thread.Sleep(100);
			Assert(SumR, 6);
			Assert(MulR, 24);

			s.onCommunication(this, "B", 9);
			Thread.Sleep(100);
			Assert(SumR, 12);
			Assert(MulR, 48);
		}
	}
}
