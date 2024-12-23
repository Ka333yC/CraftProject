using ChunkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Apart.Extensions
{
	public static class BoundsExtensions
	{
		public static BoundsInt Round(this Bounds bounds)
		{
			var min = ChunkConstantData.WorldToBlockWorldPosition(bounds.min);
			var max = ChunkConstantData.WorldToBlockWorldPosition(bounds.max);
			var size = max - min + Vector3Int.one;
			return new BoundsInt(min, size);
		}
	}
}
