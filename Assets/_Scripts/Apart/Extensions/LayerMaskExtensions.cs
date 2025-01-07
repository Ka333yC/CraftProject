using UnityEngine;

namespace _Scripts.Apart.Extensions
{
	public static class LayerMaskExtensions
	{
		public static bool Has(this LayerMask layerMask, int layer)
		{
			return (layerMask.value & (1 << layer)) > 0;
		}
	}
}
