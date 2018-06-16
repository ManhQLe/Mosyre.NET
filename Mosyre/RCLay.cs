using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre
{
	public delegate void ResponseFunc(RClay c,object connectPoint = null);
	public delegate void InitFunc(RClay c);

	public class RClay: AttribClay
	{
		Dictionary<object, List<IClay>> _contacts;
		Dictionary<object, object> _signalStore;
		List<object> _collectedPoints;

		public RClay() : this(null) {
		}

		public RClay(Dictionary<string,object> agr) : base(agr) {
			_contacts = new Dictionary<object, List<IClay>>();
			_signalStore = new Dictionary<object, object>();
			_collectedPoints = new List<object>();
			onInit();
		}

		public T GetInput<T>(object connectPoint) {
			object o = _signalStore[connectPoint];
			return o == null ? default(T) : (T)o;
		}

		public bool Stage {
			get {
				return GetAgreementProp<bool>("Stage", false);
			}
			set {
				SetAgreementProp("Stage", value);
			}
		}

		protected object this[object connectPoint] {
			get {
				return _signalStore[connectPoint];
			}
			set {						
				signalProcessing(connectPoint, value);
			}
		}

		public List<object> ConnectPoints {
			get {
				return GetAgreementProp<List<object>>("ConnectPoints", new List<object>());
			}
			set {
				_agreement["ConnectPoints"] = value;
			}
		}

		public T GetSignals<T>(object connectPoint) {
			return (T)this[connectPoint];
		}

		public override void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{

			//Check to see if it is in connectiton list
			if (_contacts.ContainsKey(atConnectionPoint) &&
				_contacts[atConnectionPoint].IndexOf(fromClay) >= 0
				)
			{
				this[atConnectionPoint] = signal;
			}			
		}

		public override void onConnection(IClay withClay, object atConnectionPoint)
		{
			List<IClay> others = _contacts.ContainsKey(atConnectionPoint)
				? _contacts[atConnectionPoint] : new List<IClay>();
			
			if (others.IndexOf(withClay)<0)
			{
				others.Add(withClay);				
			}
			_contacts[atConnectionPoint] = others;

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
					if (Stage)
						_collectedPoints.Clear();
					onResponse(connectPoint);					
				}

			}
			else // Output
			{
				List<IClay> others = _contacts[connectPoint];
				foreach (IClay c in others)
				{
					c.onCommunication(this, connectPoint, signal);
				}
			}
		}

		virtual protected void onResponse(object connectPoint)
		{
			GetAgreementProp<ResponseFunc>("Response")?.Invoke(this, connectPoint);
		}

		virtual protected void onInit()
		{
			GetAgreementProp<InitFunc>("Init")?.Invoke(this);
		}

	}
}
