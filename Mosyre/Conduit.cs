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
		public Dictionary<IClay, List<object>> _contacts;
		bool _parallelTrx = true;
		public Conduit() : this(new Dictionary<string, object>()) {
		}

		public Conduit(Dictionary<string, object> agreement) : base(agreement)
		{
			_contacts = new Dictionary<IClay, List<object>>();
		}


		public bool ParallelTrx {
			get { return _parallelTrx; }
			set { _parallelTrx = value; }
		}

		public override void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{			

			if (_contacts.ContainsKey(fromClay) && _contacts[fromClay].IndexOf(atConnectionPoint) >= 0)
			{

				foreach (IClay c in _contacts.Keys)
				{
					List<object> cps = _contacts[c];
				
					foreach (object cp in cps)
					{
						if (!cp.Equals(atConnectionPoint) || c != fromClay)
						{							
							if (ParallelTrx)							
								new Thread(() => _ThreadVibrate(this, c, cp, signal)).Start();							
							else
								c.onCommunication(this, cp, signal);							
						}							
					}
				}
			}
		}

		public override void onConnection(IClay withClay, object atConnectionPoint)
		{
			//Get all current connection with this clays
			List<object> cps = _contacts.ContainsKey(withClay)
				? _contacts[withClay]
				: new List<object>();

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

		public Conduit Connect(IClay c, object atConnectionPoint) {
			this.onConnection(c, atConnectionPoint);
			c.onConnection(this, atConnectionPoint);
			return this;
		}

		public static Conduit Link(IClay c, object atConnectionPoint){
			return new Conduit()
			.Connect(c, atConnectionPoint);			
		}

		public static Conduit Link(IClay c1, object cp1, object cp2, IClay c2) {
			return Link(c1, cp1)
				.Connect(c2, cp2);
		}
	}
}
