using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre
{
	public class Conduit : AttribClay
	{
		Dictionary<IClay, List<object>> _contacts;

		public Conduit():this(new Dictionary<string, object>()) {
		}

		public Conduit(Dictionary<string, object> agreement) : base(agreement)
		{
			_contacts = new Dictionary<IClay, List<object>>();
		}

		public object Signal {
			set { }
		}

		public override void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{
			foreach (IClay c in _contacts.Keys)
			{
				List<object> cps = _contacts[c];

				foreach (object cp in cps)
				{
					if (!cp.Equals(atConnectionPoint) || c != fromClay)
					{
						c.onCommunication(this, cp, signal);
					}						
				}
			}
		}

		public override void onConnection(IClay withClay, object atConnectionPoint)
		{
			//Get all current connection with this clays
			List<object> cps = _contacts[withClay];

			cps = cps == null ? new List<object>() : cps;

			if (cps.Count > 0 && withClay is Conduit) // Conduit only allow 1 connection
				return;

			bool shouldInclude = true;

			foreach (object cp in cps) {
				if (cp.Equals(atConnectionPoint))
				{
					shouldInclude = false;
					break;
				}
			}

			if (shouldInclude)
			{
				cps.Add(atConnectionPoint);
				_contacts[withClay] = cps;
			}
		}
	}
}
