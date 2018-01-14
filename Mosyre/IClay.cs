using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre
{
	public interface IClay
	{
		void onCommunication(IClay fromClay, object atConnectionPoint, object signal);
		void onConnection(IClay withClay, object atConnectionPoint);
	}
}
