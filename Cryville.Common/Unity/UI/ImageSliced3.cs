using System;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace Cryville.Common.Unity.UI {
	/// <summary>
	/// An image that is sliced into three parts, with the middle part stretched and the two borders preserved their aspect ratio.
	/// </summary>
	[ExecuteInEditMode]
	public class ImageSliced3 : MaskableGraphic {
		[SerializeField]
		[Tooltip("The sliced sprite.")]
		private Sprite m_sprite;
		/// <summary>
		/// The sliced sprite.
		/// </summary>
		public Sprite Sprite {
			get { return m_sprite; }
			set { m_sprite = value; }
		}

		/// <summary>
		/// The mode how a sliced image is generated when it is too compact.
		/// </summary>
		public enum CompactMode {
			/// <summary>
			/// Squeezes both the left border and the right border.
			/// </summary>
			SqueezeBoth = 0,
			/// <summary>
			/// Squeezes the left border and preserves the width of the right border.
			/// </summary>
			SqueezeLeft = 2,
			/// <summary>
			/// Squeezes the right border and preserves the width of the left border.
			/// </summary>
			SqueezeRight = 3,
			/// <summary>
			/// Squeezes the lower edge of the left border and the upper edge of the right border.
			/// </summary>
			DiagonalLeft = 4,
			/// <summary>
			/// Squeezes the upper edge of the left border and the lower edge of the right border.
			/// </summary>
			DiagonalRight = 5,
		}
		[SerializeField]
		[Tooltip("The mode how a sliced image is generated when it is too compact.")]
		private CompactMode m_compact;
		/// <summary>
		/// The mode how a sliced image is generated when it is too compact.
		/// </summary>
		public CompactMode Compact {
			get { return m_compact; }
			set { m_compact = value; }
		}

		public override Texture mainTexture {
			get { return Sprite == null ? s_WhiteTexture : Sprite.texture; }
		}

		protected override void OnPopulateMesh(VertexHelper vh) {
			float actualWidth = rectTransform.rect.width;

			Vector4 uv = DataUtility.GetOuterUV(Sprite);

			float cornerLeftSizeRatio = Sprite.border.x / Sprite.rect.height;
			float actualLeftCornerWidth = cornerLeftSizeRatio * rectTransform.rect.height;
			float actualLeftUVWidth = (uv.w - uv.y) * (Sprite.border.x / Sprite.rect.width);

			float cornerRightSizeRatio = Sprite.border.z / Sprite.rect.height;
			float actualRightCornerWidth = cornerRightSizeRatio * rectTransform.rect.height;
			float actualRightUVWidth = (uv.w - uv.y) * (Sprite.border.z / Sprite.rect.width);

			float actualTotalCornerWidth = actualLeftCornerWidth + actualRightCornerWidth;

			Vector2 corner1 = new Vector2(0f, 0f);
			Vector2 corner2 = new Vector2(1f, 1f);

			corner1.x -= rectTransform.pivot.x;
			corner1.y -= rectTransform.pivot.y;
			corner2.x -= rectTransform.pivot.x;
			corner2.y -= rectTransform.pivot.y;

			corner1.x *= actualWidth;
			corner1.y *= rectTransform.rect.height;
			corner2.x *= actualWidth;
			corner2.y *= rectTransform.rect.height;

			if (Sprite == null) {
				throw new UnityException("No sprite");
			}
			else if (Sprite.border == Vector4.zero) {
				throw new UnityException("No sprite border");
			}

			float w3, w4, w5, w6;
			if (actualTotalCornerWidth > actualWidth) {
				switch (Compact) {
					case CompactMode.SqueezeBoth:
						w3 = w4 = actualLeftCornerWidth / actualTotalCornerWidth * actualWidth;
						w5 = w6 = actualRightCornerWidth / actualTotalCornerWidth * actualWidth;
						break;
					case CompactMode.SqueezeLeft:
						w3 = w4 = actualWidth - actualRightCornerWidth;
						w5 = w6 = actualRightCornerWidth;
						break;
					case CompactMode.SqueezeRight:
						w3 = w4 = actualLeftCornerWidth;
						w5 = w6 = actualWidth - actualLeftCornerWidth;
						break;
					case CompactMode.DiagonalLeft:
						w3 = actualLeftCornerWidth;
						w4 = actualWidth - actualRightCornerWidth;
						w5 = actualWidth - actualLeftCornerWidth;
						w6 = actualRightCornerWidth;
						break;
					case CompactMode.DiagonalRight:
						w3 = actualWidth - actualRightCornerWidth;
						w4 = actualLeftCornerWidth;
						w5 = actualRightCornerWidth;
						w6 = actualWidth - actualLeftCornerWidth;
						break;
					default:
						throw new ArgumentOutOfRangeException("Compact");
				}
			}
			else {
				w3 = w4 = actualLeftCornerWidth;
				w5 = w6 = actualRightCornerWidth;
			}

			vh.Clear();
			vh.AddVert(new Vector3(corner1.x, corner2.y), color, new Vector2(uv.x, uv.w));
			vh.AddVert(new Vector3(corner1.x, corner1.y), color, new Vector2(uv.x, uv.y));
			vh.AddVert(new Vector3(corner1.x + w3, corner2.y), color, new Vector2(uv.x + actualLeftUVWidth, uv.w));
			vh.AddVert(new Vector3(corner1.x + w4, corner1.y), color, new Vector2(uv.x + actualLeftUVWidth, uv.y));
			vh.AddVert(new Vector3(corner2.x - w5, corner2.y), color, new Vector2(uv.z - actualRightUVWidth, uv.w));
			vh.AddVert(new Vector3(corner2.x - w6, corner1.y), color, new Vector2(uv.z - actualRightUVWidth, uv.y));
			vh.AddVert(new Vector3(corner2.x, corner2.y), color, new Vector2(uv.z, uv.w));
			vh.AddVert(new Vector3(corner2.x, corner1.y), color, new Vector2(uv.z, uv.y));

			if (((int)Compact & 0x1) == 0) {
				vh.AddTriangle(2, 1, 0);
				vh.AddTriangle(1, 2, 3);
				vh.AddTriangle(4, 3, 2);
				vh.AddTriangle(3, 4, 5);
				vh.AddTriangle(6, 5, 4);
				vh.AddTriangle(5, 6, 7);
			}
			else {
				vh.AddTriangle(3, 1, 0);
				vh.AddTriangle(0, 2, 3);
				vh.AddTriangle(5, 3, 2);
				vh.AddTriangle(2, 4, 5);
				vh.AddTriangle(7, 5, 4);
				vh.AddTriangle(4, 6, 7);
			}
		}
	}
}
