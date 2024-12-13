using MeshCreation.Preset;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.MeshCreation
{
	public class MeshPart
	{
		public readonly Vector3Int BlockPosition;

		public List<MeshDataPart> MeshDataParts { get; } = new List<MeshDataPart>();

		public List<Vector2> UV { get; } = new List<Vector2>();

		public int TrianglesCount { get; private set; } = 0;

		public int VerticesCount { get; private set; } = 0;

		public int UVsCount => UV.Count;

		public MeshPart(Vector3Int blockPosition)
		{
			BlockPosition = blockPosition;
		}

		public void AddMeshDataPart(MeshDataPart meshDataPart)
		{
			MeshDataParts.Add(meshDataPart);
			VerticesCount += meshDataPart.Vertices.Length;
			TrianglesCount += meshDataPart.Triangles.Length;
		}

		public void AddUV(Vector2[] uv)
		{
			for(int i = 0; i < uv.Length; i++)
			{
				UV.Add(uv[i]);
			}
		}
	}
}
