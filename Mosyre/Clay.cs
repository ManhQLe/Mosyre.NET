using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre
{
	public abstract class Clay : IClay
	{
		public abstract void onCommunication(IClay fromClay, object atConnectionPoint, object signal);

		public abstract void onConnection(IClay withClay, object atConnectionPoint);		
	}
}
