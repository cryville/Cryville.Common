using System.Collections.Generic;
using System.Diagnostics;

namespace Cryville.Common {
	public class Coroutine {
		readonly IEnumerator<float> _enumerator;
		readonly Stopwatch _stopwatch = new Stopwatch();
		public float Progress { get; private set; }
		public Coroutine(IEnumerator<float> enumerator) {
			_enumerator = enumerator;
		}
		public bool TickOnce() {
			if (!_enumerator.MoveNext()) return false;
			Progress = _enumerator.Current;
			return true;
		}
		public bool Tick(double minTime) {
			_stopwatch.Reset();
			_stopwatch.Start();
			while (_stopwatch.Elapsed.TotalSeconds < minTime) {
				if (!_enumerator.MoveNext()) return false;
				Progress = _enumerator.Current;
			}
			return true;
		}
	}
}
