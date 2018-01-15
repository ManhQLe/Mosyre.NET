using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Mosyre
{
	public class AttribClay : Clay
	{
		protected Dictionary<string,object> _agreement;		

		public AttribClay(Dictionary<string, object> agreement) {
			Agreement = agreement;
		}

		public Dictionary<string, object> Agreement { get => _agreement; set => _agreement = value; }				

		public override void onCommunication(IClay fromClay, object atConnectionPoint, object signal)
		{
		}

		public override void onConnection(IClay withClay, object atConnectionPoint)
		{
			
		}

		public T GetAgrementProp<T>(string name, T defaultValue=default(T)) {
			
			if (!_agreement.ContainsKey(name)) {
				_agreement[name] = defaultValue;
			}
			object x = _agreement[name];
			return x == null ? default(T) : (T)x;
		}

		public void SetAgrementProp(string name, object v) {
			_agreement[name] = v;
		}

	}
}
