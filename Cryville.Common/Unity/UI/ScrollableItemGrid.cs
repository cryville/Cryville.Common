using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cryville.Common.Unity.UI {
	/// <summary>
	/// A handler for loading an item.
	/// </summary>
	/// <param name="index">The zero-based index of the item.</param>
	/// <param name="gameObject">The game object for the item instantiated from the item template.</param>
	/// <returns></returns>
	public delegate bool LoadItemHandler(int index, GameObject gameObject);
	/// <summary>
	/// A scrollable grid that dynamically loads its items.
	/// </summary>
	public sealed class ScrollableItemGrid : MonoBehaviour {
		[SerializeField]
		[Tooltip("The item template.")]
		private GameObject m_itemTemplate;
		/// <summary>
		/// The item template.
		/// </summary>
		public GameObject ItemTemplate {
			get { return m_itemTemplate; }
			set { m_itemTemplate = value; OnTemplateUpdate(); }
		}
		/// <summary>
		/// The handler for loading an item.
		/// </summary>
		public LoadItemHandler LoadItem { private get; set; }

		/// <summary>
		/// Axis.
		/// </summary>
		public enum Axis {
			/// <summary>
			/// Horizontal (x) axis.
			/// </summary>
			Horizontal = 0,
			/// <summary>
			/// Vertical (y) axis.
			/// </summary>
			Vertical = 1,
		}
		[SerializeField]
		[Tooltip("The main axis.")]
		private Axis m_startAxis;
		/// <summary>
		/// The main axis.
		/// </summary>
		public Axis StartAxis {
			get { return m_startAxis; }
			set { m_startAxis = value; OnFrameUpdate(); }
		}

		[SerializeField]
		[Tooltip("The item count.")]
		private int m_itemCount = 3;
		/// <summary>
		/// The item count.
		/// </summary>
		public int ItemCount {
			get { return m_itemCount; }
			set { m_itemCount = value; OnRefresh(); }
		}

		[SerializeField]
		[Tooltip("The item count per line.")]
		private int m_lineItemCount = 3;
		/// <summary>
		/// The item count per line.
		/// </summary>
		public int LineItemCount {
			get { return m_lineItemCount; }
			set { m_lineItemCount = value; OnFrameUpdate(); }
		}

		[SerializeField]
		[Tooltip("The length of the cross axis per line.")]
		private float m_lineHeight = 100;
		/// <summary>
		/// The length of the cross axis per line.
		/// </summary>
		public float LineHeight {
			get { return m_lineHeight; }
			set { m_lineHeight = value; OnFrameUpdate(); }
		}

		private int LineCount {
			get { return Mathf.CeilToInt((float)m_itemCount / m_lineItemCount); }
		}
		private float GroupHeight {
			get { return m_lineHeight * LineCount; }
		}
		private Vector2 VisibleSize {
			get {
				Rect parentRect = ((RectTransform)transform.parent).rect;
				return new Vector2(
					m_startAxis == Axis.Horizontal ? parentRect.width : parentRect.height,
					m_startAxis == Axis.Horizontal ? parentRect.height : parentRect.width
				);
			}
		}
		/// <summary>
		/// The maximum count of visible lines.
		/// </summary>
		public int VisibleLines {
			get {
				return Mathf.CeilToInt(VisibleSize.y / m_lineHeight) + 1;
			}
		}

		private bool initialized;
		private GameObject[][] lines;
		private int[] refl;
		Vector2 cpos = new Vector2(0, 1);
		Vector2 pprectsize;

#pragma warning disable IDE0051
		void Start() {
			GetComponentInParent<ScrollRect>().onValueChanged.AddListener(OnScroll);
			initialized = true;
			OnFrameUpdate();
		}

		void Update() {
			Vector2 cprectsize = ((RectTransform)transform.parent).rect.size;
			if (cprectsize != pprectsize) {
				pprectsize = cprectsize;
				OnFrameUpdate();
			}
		}
#pragma warning restore IDE0051

		private void OnFrameUpdate() {
			if (!initialized) return;
			if (lines != null) for (int i = 0; i < lines.Length; i++)
				if (lines[i] != null) foreach (GameObject c in lines[i])
					GameObject.Destroy(c);
			lines = new GameObject[VisibleLines][];
			refl = new int[VisibleLines];
			for (int i = 0; i < refl.Length; i++) refl[i] = i;
			ResetLines();
			SetCells();
			ResizeGroup();
		}

		private void OnTemplateUpdate() {
			if (!initialized) return;
			ResetLines();
			SetCells();
		}

		private void ResetLines() {
			for (int i = 0; i < lines.Length; i++) {
				if (lines[i] != null) foreach (GameObject c in lines[i])
					GameObject.Destroy(c);
				lines[i] = new GameObject[LineItemCount];
				GenerateLine(i, refl[i]);
			}
		}

		void OnScroll(Vector2 scrpos) {
			cpos = scrpos;
			int l0 = GetFirstVisibleLine(cpos);
			int l1 = l0 + VisibleLines - 1;
			for (int l = l0; l <= l1; l++) {
				if (Array.IndexOf(refl, l) != -1) continue;
				int repl = Array.FindIndex(refl, i => i < l0 || i > l1);
				if (repl == -1) return;
				LoadLine(repl, l);
				SetCells(repl);
			}
		}

		void OnRefresh() {
			if (!initialized) return;
			int l0 = GetFirstVisibleLine(cpos);
			int l1 = l0 + VisibleLines - 1;
			int i = 0;
			for (int l = l0; l <= l1; i++, l++) {
				LoadLine(i, l);
				SetCells(i);
			}
			ResizeGroup();
		}

		void ResizeGroup() {
			((RectTransform)transform).sizeDelta = m_startAxis == 0
				? new Vector2(0, GroupHeight)
				: new Vector2(GroupHeight, 0);
		}

		int GetFirstVisibleLine(Vector2 scrpos) {
			float scr = m_startAxis == Axis.Horizontal ? 1 - scrpos.y : scrpos.x;
			float maxScroll = Mathf.Max(0, GroupHeight - VisibleSize.y);
			if (maxScroll == 0) return 0;
			return Mathf.FloorToInt(scr * maxScroll / LineHeight);
		}

		void GenerateLine(int index, int line) {
			for (int j = 0; j < LineItemCount; j++) {
				var child = GameObject.Instantiate(m_itemTemplate, transform, false);
				lines[index][j] = child;
			}
			LoadLine(index, line);
		}

		void LoadLine(int index, int line) {
			refl[index] = line;
			for (int j = 0; j < LineItemCount; j++) {
				var id = line * m_lineItemCount + j;
				if (id < 0 || id >= m_itemCount || LoadItem == null) lines[index][j].SetActive(false);
				else lines[index][j].SetActive(LoadItem(id, lines[index][j]));
			}
		}

		private void SetCells() {
			for (int i = 0; i < VisibleLines; i++) SetCells(i);
		}

		private void SetCells(int index) {
			float itemWidth = VisibleSize[0] / m_lineItemCount;
			for (int j = 0; j < LineItemCount; j++) {
				((RectTransform)lines[index][j].transform).anchoredPosition = m_startAxis == 0
					? new Vector2(j * itemWidth, -refl[index] * m_lineHeight)
					: new Vector2(refl[index] * m_lineHeight, -j * itemWidth);
				((RectTransform)lines[index][j].transform).sizeDelta = m_startAxis == 0
					? new Vector2(itemWidth, m_lineHeight)
					: new Vector2(m_lineHeight, itemWidth);
			}
		}
	}
}
