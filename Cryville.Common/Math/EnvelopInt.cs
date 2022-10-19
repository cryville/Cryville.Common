namespace Cryville.Common {
	/// <summary>
	/// Represents a two-dimentional axis-aligned bounding box with all values as integers.
	/// </summary>
	public struct EnvelopInt {
		/// <summary>
		/// Minimum X (inclusive).
		/// </summary>
		public int MinX { get; set; }
		/// <summary>
		/// Minimum Y (inclusive).
		/// </summary>
		public int MinY { get; set; }
		/// <summary>
		/// Maximum X (inclusive).
		/// </summary>
		public int MaxX { get; set; }
		/// <summary>
		/// Maximum Y (inclusive).
		/// </summary>
		public int MaxY { get; set; }
		/// <summary>
		/// Center X.
		/// </summary>
		public float CenterX { get { return (MinX + MaxX) / 2f; } }
		/// <summary>
		/// Center Y.
		/// </summary>
		public float CenterY { get { return (MinY + MaxY) / 2f; } }
		/// <summary>
		/// The width.
		/// </summary>
		public float Width { get { return MaxX - MinX + 1; } }
		/// <summary>
		/// The height.
		/// </summary>
		public float Height { get { return MaxY - MinY + 1; } }
		/// <summary>
		/// Expands the envelop to include a point.
		/// </summary>
		public void Include(int x, int y) {
			if (x < MinX) MinX = x;
			if (y < MinY) MinY = y;
			if (x > MaxX) MaxX = x;
			if (y > MaxY) MaxY = y;
		}
		/// <summary>
		/// Checks whether a point is included in the envelop.
		/// </summary>
		/// <returns>Whether the point is included in the envelop.</returns>
		public bool Includes(int x, int y) {
			return x >= MinX && y >= MinY && x <= MaxX && y <= MaxY;
		}
		/// <summary>
		/// Creates an envelop that contains a single point.
		/// </summary>
		public EnvelopInt(int x, int y) {
			MinX = MaxX = x;
			MinY = MaxY = y;
		}
	}
}
