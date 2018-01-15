using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre
{
	public delegate void ResponseFunc(ResponsiveClay c,object connectPoint = null);
	public delegate void InitFunc(ResponsiveClay c);

	public class ResponsiveClay: AttribClay
	{
		Dictionary<object, IClay> _contacts;
		Dictionary<object, object> _signalStore;
		List<object> _collectedPoints;
		public ResponsiveClay(Dictionary<string,object> agr) : base(agr) {
			_contacts = new Dictionary<object, IClay>();
			_signalStore = new Dictionary<object, object>();
			_collectedPoints = new List<object>();

		}

		public T GetInput<T>(object connectPoint) {
			object o = _signalStore[connectPoint];
			return o == null ? default(T) : (T)o;
		}

		public InitFunc Init {
			get { return GetAgrementProp<InitFunc>("Init", defaultInit); }
			set {
				_agreement["Init"] = value;
			}
		}

		public ResponseFunc Response {
			get {
				return GetAgrementProp<ResponseFunc>("Response", defaultResponse);
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
						

				signalProcessing(connectPoint, value);
				
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
			if (_contacts[atConnectionPoint] == fromClay) {
				this[atConnectionPoint] = signal;
			}
		}

		public override void onConnection(IClay withClay, object atConnectionPoint)
		{
			_contacts[atConnectionPoint] = withClay;
		}

		private void signalProcessing(object connectPoint, object signal) {
			List<Object> cps = ConnectPoints;
			//This is important to check for fire stage		
			bool isInput = cps.IndexOf(connectPoint) >= 0;
			if (isInput)
			{				
				_signalStore[connectPoint] = signal;
				//Check if collected point ?
				bool hasCollected = _collectedPoints.IndexOf(connectPoint) >= 0;
				if (!hasCollected)
					_collectedPoints.Add(connectPoint);

				if (_collectedPoints.Count == cps.Count)
				{
					Response(this, connectPoint);
					if (Stage)
						_collectedPoints.Clear();
				}

			}
			else // Output
			{
				IClay o = _contacts[connectPoint];
				o?.onCommunication(this, connectPoint, signal);
			}
		}

		static private void defaultResponse(ResponsiveClay c, object connectPoint)
		{
			
		}
		static private void defaultInit(ResponsiveClay c)
		{

		}

	}
}
