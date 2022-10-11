using UnityEngine;
using UnityEngine.UI;

namespace Cryville.Common.Unity.UI {
	/// <summary>
	/// Fits the size of a <see cref="GridLayoutGroup" /> with its cells.
	/// </summary>
	[RequireComponent(typeof(GridLayoutGroup))]
	public class GridLayoutSizeFitter : MonoBehaviour {
		RectTransform rectTransform;
		GridLayoutGroup gridLayoutGroup;
		Canvas canvas;
		/// <summary>
		/// The item count per line.
		/// </summary>
		public int GroupItemCount = 3;

#pragma warning disable IDE0051
		void Awake() {
			rectTransform = GetComponent<RectTransform>();
			gridLayoutGroup = GetComponent<GridLayoutGroup>();
			canvas = GetComponentInParent<Canvas>();
		}
		void Update() {
			var cellSize = gridLayoutGroup.cellSize;
			var frameSize = rectTransform.sizeDelta;
			int lineCount = Mathf.CeilToInt((float)transform.childCount / GroupItemCount);
			var rect = RectTransformUtility.PixelAdjustRect((RectTransform)transform, canvas);
			if (gridLayoutGroup.startAxis == GridLayoutGroup.Axis.Horizontal) {
				cellSize.x = rect.width / GroupItemCount;
				frameSize.y = cellSize.y * lineCount;
			}
			else {
				cellSize.y = rect.height / GroupItemCount;
				frameSize.x = cellSize.x * lineCount;
			}
			gridLayoutGroup.cellSize = cellSize;
			rectTransform.sizeDelta = frameSize;
		}
#pragma warning restore IDE0051
	}
}
