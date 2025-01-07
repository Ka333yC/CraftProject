using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

namespace _Scripts.Core.MeshWrap
{
	public class MeshBuilder : IDisposable
	{
		private readonly List<MeshPart> _meshParts;

		private int _verticesCount = 0;
		private int _trianglesCount = 0;
		private int _uvsCount = 0;
		private bool _isRented = false;
		private Vector3[] _vertices;
		private int[] _triangles;
		private Vector2[] _uv;
		private Vector3[] _normals;

		public MeshBuilder()
		{
			_meshParts = ListPool<MeshPart>.Get();
		}

		public void Dispose()
		{
			_meshParts.Clear();
			ListPool<MeshPart>.Release(_meshParts);
			if(_isRented)
			{
				ArrayPool<int>.Shared.Return(_triangles);
				ArrayPool<Vector3>.Shared.Return(_vertices);
				ArrayPool<Vector2>.Shared.Return(_uv);
				ArrayPool<Vector3>.Shared.Return(_normals);
			}
		}

		public void Add(MeshPart meshPart)
		{
			_meshParts.Add(meshPart);
			_verticesCount += meshPart.VerticesCount;
			_trianglesCount += meshPart.TrianglesCount;
			_uvsCount += meshPart.UVsCount;
		}

		public void AddRange(IEnumerable<MeshPart> meshParts)
		{
			foreach(var meshPart in meshParts)
			{
				Add(meshPart);
			}
		}

		public void Bake()
		{
			_vertices = ArrayPool<Vector3>.Shared.Rent(_verticesCount);
			_triangles = ArrayPool<int>.Shared.Rent(_trianglesCount);
			_uv = ArrayPool<Vector2>.Shared.Rent(_uvsCount);
			_normals = ArrayPool<Vector3>.Shared.Rent(_verticesCount);
			_isRented = true;
			FillMeshArrays();
			RecalculateNormals();
		}

		public Mesh CreateMesh()
		{
			Mesh result = new Mesh();
			result.indexFormat = IndexFormat.UInt32;
			result.SetVertices(_vertices, 0, _verticesCount);
			result.SetTriangles(_triangles, 0, _trianglesCount, 0);
			result.SetUVs(0, _uv, 0, _uvsCount);
			result.SetNormals(_normals, 0, _verticesCount);
			return result;
		}

		private void FillMeshArrays()
		{
			int trianglesIndex = 0;
			int verticesIndex = 0;
			int uvIndex = 0;
			for(int i = 0; i < _meshParts.Count; i++)
			{
				// Заполняем trinales и vertices
				var meshPart = _meshParts[i];
				var blockPosition = meshPart.BlockPosition;
				var meshDataParts = meshPart.MeshDataParts;
				for(int j = 0; j < meshDataParts.Count; j++)
				{
					var meshDataPart = meshDataParts[j];
					var triangles = meshDataPart.Triangles;
					for(int k = 0; k < triangles.Length; k++)
					{
						_triangles[trianglesIndex++] = triangles[k] + verticesIndex;
					}

					var vertices = meshDataPart.Vertices;
					for(int k = 0; k < vertices.Length; k++)
					{
						_vertices[verticesIndex++] = vertices[k] + blockPosition;
					}
				}

				// Заполняем uv
				var uv = meshPart.UV;
				for(int j = 0; j < uv.Count; j++)
				{
					_uv[uvIndex++] = uv[j];
				}
			}
		}

		private void RecalculateNormals() 
		{
			for(int i = 0; i < _trianglesCount; i += 3)
			{
				var a = _vertices[_triangles[i]];
				var b = _vertices[_triangles[i + 1]];
				var c = _vertices[_triangles[i + 2]];
				var normal = Vector3.Cross(b - a, c - a).normalized;
				_normals[_triangles[i]] = normal;
				_normals[_triangles[i + 1]] = normal;
				_normals[_triangles[i + 2]] = normal;
			}
		}
	}
}
