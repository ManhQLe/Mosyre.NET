using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre
{
	public delegate void ResponseFunc(ResponsiveClay c);
	

	public class ResponsiveClay: AttribClay
	{
		Dictionary<object, IClay> _contacts;
		Dictionary<object, object> _signalStore;
		public ResponsiveClay(Dictionary<string,object> agr) : base(agr) {
			_contacts = new Dictionary<object, IClay>();
			_signalStore = new Dictionary<object, object>();


		}

		public ResponseFunc Response {
			get {
				return GetAgrementProp<ResponseFunc>("Response", defaultFunction);
			}
			set {
				_agreement["Response"] = value;
			}
		}

		public bool Stage {
			get {
				return GetAgrementProp<bool>("Stage", false);
			}
			set {
				SetAgrementProp("Stage", value);
			}
		}

		public object this[object connectPoint] {
			get {
				return _signalStore[connectPoint];
			}
			set {
				//This is important to check for fire stage
				//Should be thread
			}
		}

		public List<object> ConnectPoints {
			get {
				return GetAgrementProp<List<object>>("ConnectPoints", new List<object>());
			}
			set {
				_agreement["ConnectPoints"] = value;
			}
		}

		public override void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{			
			//Check to see if it is in connectiton list

		}

		public override void onConnection(IClay withClay, object atConnectionPoint)
		{
			_contacts[atConnectionPoint] = withClay;
		}

		static private void defaultFunction(ResponsiveClay c)
		{

		}
	}
}
