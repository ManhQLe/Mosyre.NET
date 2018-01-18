using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre
{
	public class Conduit : AttribClay
	{
		List<KeyValuePair<object, IClay>> _contacts;

		public Conduit():this(new Dictionary<string, object>()) {
		}

		public Conduit(Dictionary<string, object> agreement) : base(agreement)
		{
			_contacts = new List<KeyValuePair<object, IClay>>();
		}

		public object Signal {
			set { }
		}

		public override void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{
			
		}

		public override void onConnection(IClay withClay, object atConnectionPoint)
		{
			int dix = _contacts.fin
		}
	}
}
