using SMath = System.Math;

namespace Cryville.Common.Math {
	// Ported from https://github.com/arian/cubic-bezier/blob/master/index.js
	public static class CubicBezier {
		public static float Evaluate(float t, float x1, float y1, float x2, float y2, float epsilon) {
			float x = t, t0 = 0, t1 = 1, t2 = x;

			if (t2 < t0) return Curve(t0, y1, y2);
			if (t2 > t1) return Curve(t1, y1, y2);

			while (t0 < t1) {
				float tx = Curve(t2, x1, x2);
				if (SMath.Abs(tx - x) < epsilon) return Curve(t2, y1, y2);
				if (x > tx) t0 = t2;
				else t1 = t2;
				t2 = (t1 - t0) * .5f + t0;
			}

			return Curve(t2, y1, y2);
		}
		static float Curve(float t, float p1, float p2) {
			float v = 1 - t;
			return 3 * v * v * t * p1 + 3 * v * t * t * p2 + t * t * t;
		}
	}
}
