using UnityEngine;

namespace _Scripts.Apart.Extensions
{
	public static class RectExtensions
	{
		public static Vector2[] GetEdges(this Rect rect)
		{
			var result = new Vector2[4];
			result[0] = new Vector2(rect.xMin, rect.yMin);
			result[1] = new Vector2(rect.xMin, rect.yMax);
			result[2] = new Vector2(rect.xMax, rect.yMin);
			result[3] = new Vector2(rect.xMax, rect.yMax);

			return result;
		}
	}
}
