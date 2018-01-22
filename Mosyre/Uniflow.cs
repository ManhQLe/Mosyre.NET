using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre
{	
	public class Uniflow : AttribClay
	{
		public Uniflow() : this(null) { }

		public Uniflow(Dictionary<string, object> agreement) : base(agreement)
		{
		}

		public override void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{
			
		}

		public override void onConnection(IClay withClay, object atConnectionPoint)
		{
			if (atConnectionPoint.Equals(IN)) {
			}
		}

		static int IN = 0;
		static int OUT = 1;

	}
}
