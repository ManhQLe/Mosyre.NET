using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre
{	
	delegate List<SClayLayout> SClayBuildFx(IClay clay);

	public class SClay : AttribClay
	{
		protected List<SClayLayout> _map;
		protected Dictionary<object, Conduit> _contacts = new Dictionary<object, Conduit>();
		public SClay() : this(null) {

		}

		public SClay(Dictionary<string, object> agreement) : base(agreement)
		{

			List<SClayLayout> _map = this.onBuild();			

			foreach (var e in _map) {
				Conduit con = null;
				if (_contacts.ContainsKey(e.HostConnectPoint))
				{
					con = _contacts[e.HostConnectPoint];
				}
				else
				{
					con = new Conduit();
					con.Link(this, e.HostConnectPoint);
					_contacts[e.HostConnectPoint] = con;
				}
				con.Link(e.WithClay, e.AtConnectionPoint);
			}

		}

		public object this[object connectPoint]
		{	
			set
			{
				if (_contacts.ContainsKey(connectPoint)) {
					_contacts[connectPoint].onCommunication(this, connectPoint, value);
				}

			}
		}

		public List<SClayLayout> LayoutMap {
			get {
				return GetAgreementProp<List<SClayLayout>>("layoutMap",new List<SClayLayout>());
			}
			set {
				SetAgreementProp("layoutMap", value);
			}
		}

		public override void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{		
			
		}


		public override void onConnection(IClay withClay, object atConnectionPoint)
		{
			if (_contacts.ContainsKey(atConnectionPoint)) {
				_contacts[atConnectionPoint].Link(withClay, atConnectionPoint);
			}
		}

		protected List<SClayLayout> onBuild() {
			return GetAgreementProp<SClayBuildFx>("build",(c)=> { return LayoutMap; })?.Invoke(this);
		}
	}
}
