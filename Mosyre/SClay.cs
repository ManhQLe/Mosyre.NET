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
		protected List<object> contactPoints;
		protected List<SClayLayout> _map;
		protected Dictionary<object, List<IClay>> _contacts = new Dictionary<object, List<IClay>>();
		public SClay() : this(null) {

		}

		public SClay(Dictionary<string, object> agreement) : base(agreement)
		{
			contactPoints = new List<object>();
			_map= this.onBuild();			

			foreach (var e in _map) {
				this.contactPoints.Add(e.HostConnectPoint);
				Clay.MakeConnection(this, e.WithClay, e.HostConnectPoint, e.AtConnectionPoint);
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
			var r = _map.Find(m => m.WithClay == fromClay && m.AtConnectionPoint == atConnectionPoint);
			//From Map			
			if (r != null)
			{
				if (_contacts.ContainsKey(r.HostConnectPoint))
				{
					var clays = _contacts[r.HostConnectPoint].FindAll(c => c != fromClay);
					var internals = _map.FindAll(m => clays.IndexOf(m.WithClay) >= 0 && m.HostConnectPoint == r.HostConnectPoint);

					var outs = clays.Where(x => internals.FindIndex(m => m.WithClay == x) < 0);

					foreach (var m in internals) {
						m.WithClay.onCommunication(this, m.AtConnectionPoint, signal);
					}

					foreach (var c in outs)
					{						
						c.onCommunication(this, r.HostConnectPoint, signal);
					}
				}

			}
			else {				

				var clays = _contacts[atConnectionPoint];
				if (clays != null) {
					foreach (var m in _map) {						
						if (m.HostConnectPoint == atConnectionPoint) {							
							m.WithClay.onCommunication(this, m.AtConnectionPoint, signal);
						}
					}
				}
			}
		}


		public override void onConnection(IClay withClay, object atConnectionPoint)
		{
			List<IClay> clays;

			if (_contacts.ContainsKey(atConnectionPoint))
			{
				clays = _contacts[atConnectionPoint];
			}
			else
			{
				clays = new List<IClay>();
			}

			if (clays.FindIndex(c => c == withClay) <= 0) {
				clays.Add(withClay);
			}

			_contacts[atConnectionPoint] = clays;
		}

		protected List<SClayLayout> onBuild() {
			return GetAgreementProp<SClayBuildFx>("build",(c)=> { return LayoutMap; })?.Invoke(this);
		}
	}
}
