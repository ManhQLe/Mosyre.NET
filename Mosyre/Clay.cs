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

		public static void MakeConnection(IClay c1, IClay c2, object cp1, object cp2 = null) {
			c2.onConnection(c1, cp2 ?? cp1);
			c1.onConnection(c2, cp1);
		}
	}
}
