using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosyre
{
	public class Conduit : AttribClay
	{
		Dictionary<IClay, List<object>> _contacts;
		bool _parallelTrx = false;
		public Conduit():this(new Dictionary<string, object>()) {
		}

		public Conduit(Dictionary<string, object> agreement) : base(agreement)
		{
			_contacts = new Dictionary<IClay, List<object>>();
		}

		public object Signal {
			set { }
		}

		public bool ParallelTrx {
			get { return _parallelTrx;}
			set { _parallelTrx = true; }
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
						if (ParallelTrx)
						{
							c.onCommunication(this, cp, signal);
							new Thread(() => _ThreadVibrate(this,c, cp, signal)).Start();
						}
						else
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

		static void _ThreadVibrate(IClay from, IClay target, object cp, object signal) {
			target.onCommunication(from, cp, signal);
		}
	}
}
