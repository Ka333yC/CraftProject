using System;
using UnityEngine;

namespace MeshCreation.Preset
{
	[Serializable]
	public class MeshDataPart
	{
		public static readonly MeshDataPart FullForwardSide = new MeshDataPart()
		{
			Triangles = new int[6] { 0, 1, 2, 2, 1, 3 },
			Vertices = new Vector3[4]
			{
				new Vector3(1, 0, 1),
				new Vector3(1, 1, 1),
				new Vector3(0, 0, 1),
				new Vector3(0, 1, 1),
			}
		};

		public static readonly MeshDataPart FullBackSide = new MeshDataPart()
		{
			Triangles = new int[6] { 0, 1, 2, 2, 1, 3 },
			Vertices = new Vector3[4]
			{
				new Vector3(0, 0, 0),
				new Vector3(0, 1, 0),
				new Vector3(1, 0, 0),
				new Vector3(1, 1, 0),
			}
		};

		public static readonly MeshDataPart FullUpSide = new MeshDataPart()
		{
			Triangles = new int[6] { 0, 1, 2, 2, 1, 3 },
			Vertices = new Vector3[4]
			{
				new Vector3(0, 1, 0),
				new Vector3(0, 1, 1),
				new Vector3(1, 1, 0),
				new Vector3(1, 1, 1),
			}
		};

		public static readonly MeshDataPart FullDownSide = new MeshDataPart()
		{
			Triangles = new int[6] { 0, 1, 2, 2, 1, 3 },
			Vertices = new Vector3[4]
			{
				new Vector3(1, 0, 0),
				new Vector3(1, 0, 1),
				new Vector3(0, 0, 0),
				new Vector3(0, 0, 1),
			}
		};

		public static readonly MeshDataPart FullRightSide = new MeshDataPart()
		{
			Triangles = new int[6] { 0, 1, 2, 2, 1, 3 },
			Vertices = new Vector3[4]
			{
				new Vector3(1, 0, 0),
				new Vector3(1, 1, 0),
				new Vector3(1, 0, 1),
				new Vector3(1, 1, 1),
			}
		};

		public static readonly MeshDataPart FullLeftSide = new MeshDataPart()
		{
			Triangles = new int[6] { 0, 1, 2, 2, 1, 3 },
			Vertices = new Vector3[4]
			{
				new Vector3(0, 0, 1),
				new Vector3(0, 1, 1),
				new Vector3(0, 0, 0),
				new Vector3(0, 1, 0),
			}
		};

		public int[] Triangles;
		public Vector3[] Vertices;
	}
}
