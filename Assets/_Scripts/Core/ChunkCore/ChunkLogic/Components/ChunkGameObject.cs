using UnityEngine;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Components
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
	public class ChunkGameObject : MonoBehaviour
	{
		private Vector3Int _gridPosition;

		public MeshFilter MeshFilter { get; private set; }
		public MeshCollider MeshCollider { get; private set; }
		public Vector3Int GridPosition
		{
			get
			{
				return _gridPosition;
			}

			set
			{
				_gridPosition = value;
				transform.position = ChunkConstantData.GridToWorldPosition(_gridPosition);
				name = $"Chunk {_gridPosition.x} {_gridPosition.z}";
			}
		}

		private void Awake()
		{
			MeshFilter = GetComponent<MeshFilter>();
			MeshCollider = GetComponent<MeshCollider>();
		}
	}
}
